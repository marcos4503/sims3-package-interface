# REPOSITORY NOTICE

> Nowadays it is very difficult to find solid information and real help regarding S3PI, even more so if you are trying to create a program using S3PI as a library to interact with The Sims Package files. Furthermore, the S3PI library is not on GitHub and does not have a repository here.
>
> For these reasons, I decided to create this Repository to store the S3PI library in its entirety, with code examples and tips on how to use it correctly. In addition, I have compiled here all the useful information I found on the Internet related to S3PI!
>
> If you would like to contribute more information, please create a Pull Request or report it via the "Issues" tab. I will do my best to keep this repository with as much information and examples as I can, but if the author of S3PI requests it, I will take this repository offline.

> [!TIP]
> Remembering that ALL credits for the creation of the S3PI library go to the incredible "<a href="https://sourceforge.net/u/pljones/profile/">Peter L Jones</a>"! 🙂

# What is S3PI?

The "s3pi" is an acronym for "Sims3™ Package Interface". It provides support for accessing the data within individual "package" files used by Electronic Arts Sims3™ game. A "package" is a disk file with a file extension usually of ".package" (but other extensions are used). However, the main identifying feature used by s3pi is the four byte magic cookie at the start of the file, which must be "DBPF". In addition, the file format version number must be 2 for Sims 3™ (or 3 for Sim City 5™, which is barely supported).

Note that "protected" packages (with a magic cookie of "DBPP") are not supported. Note that "temporary" packages (with a magic cookie of "DBBF") are not currently supported.

The s3pi is a set of .Net assemblies (documented below and available here) that implement the descriptions from the <a href="https://simswiki.info/wiki.php?title=Tutorials:TS3_Advanced_Coding_Tutorials">Sims 3 Wiki</a> (with these pages alternatively <a href="https://marcos4503.github.io/sims3-package-interface/Repository-Pages/sims3-wiki-files/index-of.html">available here</a> as wiki-mirror). Note that it is always worth checking there if you are unsure of something, either about how something should work or if you think you have found a problem in s3pi, which remains quite possible. Neither the wiki nor the library is always right - indeed, it can be that both are wrong in different ways.

# S3PI Help

The s3pi library comes with a compiled help file (.chm) that describes much of what you need to know. You can also access a version of this online, <a href="http://peter.drealm.info/Help/Index">here</a>. This page tries to give a quick overview, as the library is pretty extensive and it may be tricky working out what it is you do need to know.

This page is divided into three remaining sections.

- The guide to the core library components, intended to provide some idea of the support provided by the library, with the exclusion of the resource handlers ("wrappers").
- A section on writing a wrapper, for those who want to extend the library further.
- And finally, a brief section on RCOL resources and how to extend the library to support them.
- How to think about using S3PI, and some C# sample code

Once you understand how to use a wrapper to access data in a package, you should consult the Sims3 Wiki <a href="https://simswiki.info/wiki.php?title=Sims_3:PackedFileTypes">list of file types</a> to see which are supported and how to get the support (if not part of the s3pi distribution).

> [!TIP]
> If you have unanswered questions, please feel free to post here in the <a href="https://github.com/marcos4503/sims3-package-interface/discussions">Forum/Discussions</a>. It will be a pleasure to help!

# Using the S3PI Library With Your Tool

The library is a set of <a href="https://en.wikipedia.org/wiki/Assembly_(CLI)">.NET Assembly</a> DLLs. If you are likely to work on more than one project using the library, I strongly advise putting them in a folder separate from your project and solution workspaces - but close by in your folder structure. This way, it's easy to replace them all without having to update each project.

To use the library from a new project, you first need to know which parts of the library you are going to use. I have deliberately kept the dependant parts separate so that your project only needs to reference the DLLs it needs, hence keeping your project down in size. The downside is there are lots of DLLs!

The first thing to notice is that many of them are "wrappers" - anything with a name like `SomethingResource.DLL`. These contain the code to understand the content of one or more resources within a Sims 3 Package file.

First, let's take a look at each of the remaining assemblies. After this, I'll provide an outline of how to start a new project. I'll end with some thoughts on organising your working practices.

> [!NOTE]
> You must agree to the GPLv3 licence before distributing your code. Notice that, as you're linking to a GPLv3 library, you are essentially re-using it and must distribute your code under equivalent terms. You can find further details on <a href="https://www.fsf.org/">The Free Software Foundation's</a> site.

## Overview of the Assemblies

### System.Custom

**When to include it:** Always - it's required by several other parts of library.

**Summary:** Contains a number of classes that do not directly relate to "The Sims 3" and have no references to any of the `s3pi.Interfaces` types. They could, conceivably be of use for projects not using the rest of the s3pi library (hence keeping it separate).

**Further reading:** By referencing this assembly, you get the following...

- `System.Collections.Generic.AHandlerList<T>` - Abstract extension of `List<T>` providing feedback on list updates through a supplied `EventHandler`.
- `System.ArgumentLengthException` - Represents an error in the length of an argument to a method.
- `System.Extensions` - Useful Extension methods not provided by Linq (and without deferred execution). (The class name is dubious and may change...)
- `System.Security.Cryptography.FNV32` - FNV32 hash routine
- `System.Security.Cryptography.FNV64` - FNV64 hash routine
- `System.Text.SevenBitString` - Read and write a seven-bit encoded length-prefixed string in a given `Encoding` from or to a `Stream`.
- `System.Security.Cryptography.Sims3PackCRC` - Calculate the CRC of a data chunk stored in a Sims3Pack file. (OK, arguably this has no reason being here but in terms of code dependencies, it makes sense!)

### s3pi.Settings

**When to include it:** Always.

**Summary:** Originally intended to allow various settings internal to the library to be changed by replacing the DLL. This has never happened.

**Further reading:** Nothing.

### s3pi.Interfaces

**When to include it:** Always. Not only is it required by the library itself but it defines the public API.

**Summary:** Provides a number of Interfaces, abstract Classes and helper Classes used throughout the library and wrappers, which define the public methods provided by the various library classes.

**Further reading:**

- `s3pi.Interfaces` namespace documentation.

Note that the namespace is "polluted" by classes from `s3pi.GenericRCOL`. Purists might also consider some of the helper classes "pollution"...