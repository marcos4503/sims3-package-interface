# REPOSITORY NOTICE!

```
Nowadays it is very difficult to find solid information and real help regarding S3PI, even more so if you are trying to create a program using S3PI as a library to interact with The Sims Package files. Furthermore, the S3PI library is not on GitHub and does not have a repository here.

For these reasons, I decided to create this Repository to store the S3PI library in its entirety, with code examples and tips on how to use it correctly. In addition, I have compiled here all the useful information I found on the Internet related to S3PI!

If you would like to contribute more information, please create a Pull Request or report it via the "Issues" tab. I will do my best to keep this repository with as much information and examples as I can, but if the author of S3PI requests it, I will take this repository offline.
```

> [!TIP]
> Remembering that ALL credits for the creation of the S3PI library go to the incredible "<a href="https://sourceforge.net/u/pljones/profile/">Peter L Jones</a>"! 🙂

# Basics

The "Sims 3 Package Interface" provides a core library of portable code that "understands" Sims3 game packages. Note that (with some minor tweaks) the core library code also understands other game package formats (e.g. SimCity Online, Sims4).

Along with the core library are a number of "wrappers" that provide the main part of the project. These handle deserialization and serialization of data within the package (or any other source).

Further tools can use this library and wrappers to manipulate the data content of Package files of Sims games.

Note that development on this library and wrappers provided here has now finished.

# What is S3PI?

The "s3pi" is an acronym for "Sims3™ Package Interface". It provides support for accessing the data within individual "package" files used by Electronic Arts Sims3™ game. A "package" is a disk file with a file extension usually of ".package" (but other extensions are used). However, the main identifying feature used by s3pi is the four byte magic cookie at the start of the file, which must be "DBPF". In addition, the file format version number must be 2 for Sims 3™ (or 3 for Sim City 5™, which is barely supported).

Note that "protected" packages (with a magic cookie of "DBPP") are not supported. Note that "temporary" packages (with a magic cookie of "DBBF") are not currently supported.

The s3pi is a set of .Net assemblies (documented below and available here) that implement the descriptions from the <a href="https://simswiki.info/wiki.php?title=Tutorials:TS3_Advanced_Coding_Tutorials">Sims 3 Wiki</a> (with these pages alternatively <a href="https://marcos4503.github.io/sims3-package-interface/Repository-Pages/sims3-wiki-files/index-of.html">available here</a> as wiki-mirror). Note that it is always worth checking there if you are unsure of something, either about how something should work or if you think you have found a problem in s3pi, which remains quite possible. Neither the wiki nor the library is always right - indeed, it can be that both are wrong in different ways.

# S3PI Help

The s3pi library comes with a compiled help file (.chm) that describes much of what you need to know. You can also access a version of this, <a href="https://marcos4503.github.io/sims3-package-interface/Repository-Pages/s3pi-files/library-compiled-help-file.chm">here</a>. This page tries to give a quick overview, as the library is pretty extensive and it may be tricky working out what it is you do need to know.

This page is divided into three remaining sections.

- The guide to the core library components, intended to provide some idea of the support provided by the library, with the exclusion of the resource handlers ("wrappers").
- A section on writing a wrapper, for those who want to extend the library further.
- And finally, a brief section on RCOL resources and how to extend the library to support them.
- How to think about using S3PI, and some C# sample code

Once you understand how to use a wrapper to access data in a package, you should consult the Sims3 Wiki <a href="https://marcos4503.github.io/sims3-package-interface/Repository-Pages/sims3-wiki-files/Files%20By%20Type%20Code.html">list of file types</a> to see which are supported and how to get the support (if not part of the s3pi distribution).

> [!TIP]
> If you have unanswered questions, please feel free to post here in the <a href="https://github.com/marcos4503/sims3-package-interface/discussions">Forum/Discussions</a>. It will be a pleasure to help!

# Using the S3PI Library With Your Tool

The library is a set of <a href="https://en.wikipedia.org/wiki/Assembly_(CLI)">.NET Assembly</a> DLLs. If you are likely to work on more than one project using the library, I strongly advise putting them in a folder separate from your project and solution workspaces - but close by in your folder structure. This way, it's easy to replace them all without having to update each project.

To use the library from a new project, you first need to know which parts of the library you are going to use. I have deliberately kept the dependant parts separate so that your project only needs to reference the DLLs it needs, hence keeping your project down in size. The downside is there are lots of DLLs!

The first thing to notice is that many of them are "wrappers" - anything with a name like `SomethingResource.DLL`. These contain the code to understand the content of one or more resources within a Sims 3 Package file.

First, let's take a look at each of the remaining assemblies. After this, I'll provide an outline of how to start a new project. I'll end with some thoughts on organising your working practices.

> [!NOTE]
> You must agree to the GPLv3 licence before distributing your code. Notice that, as you're linking to a GPLv3 library, you are essentially re-using it and must distribute your code under equivalent terms. You can find further details on <a href="https://www.fsf.org/">The Free Software Foundation's</a> site.

# Overview of the Assemblies

## System.Custom

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

## s3pi.Settings

**When to include it:** Always.

**Summary:** Originally intended to allow various settings internal to the library to be changed by replacing the DLL. This has never happened.

**Further reading:** Nothing.

## s3pi.Interfaces

**When to include it:** Always. Not only is it required by the library itself but it defines the public API.

**Summary:** Provides a number of Interfaces, abstract Classes and helper Classes used throughout the library and wrappers, which define the public methods provided by the various library classes.

**Further reading:**

- `s3pi.Interfaces` namespace documentation.

Note that the namespace is "polluted" by classes from `s3pi.GenericRCOL`. Purists might also consider some of the helper classes "pollution"...

## s3pi.Package

**When to include it:** When working with Sims 3 package files.

**Summary:** Provides the concrete implementation of the abstract classes and interfaces defined in `s3pi.Interfaces`.

**Further reading:** Nothing.

## s3pi.WrapperDealer

**When to include it:** When working with resource wrappers.

**Summary:** If you are creating new resoureces or reading resources from a package, this is the recommended mechanism. However, alternatives do exist.

**Further reading:**

- `s3pi.WrapperDealer.WrapperDealer` - Responsible for associating `ResourceType` in the `IResourceIndexEntry` with a particular class (a "wrapper") that understands it or the default wrapper.

The `s3pi.DefaultResource` and `s3pi.GenericRCOLResource` provide the basics for understanding how to use a resource once you have a reference to it.

## s3pi.Extensions

**When to include it:** If you want community standard file names.

**Summary:** At the start of The Sims 3, the modding community agreed a set format for how a packaged resource should be named when outside a package file. This assembly provides the implementation for the s3pi library.

**Further reading:** `s3pi.Extensions` namespace. This area of the library still needs properly documenting.

## s3pi.CustomForms

**When to include it:** When you want CopyableMessageBox (or IssueException).

**Summary:** This assembly provides a way of displaying messages that allows the user to copy the content easily. In future, other general controls may appear here.

**Further reading:** `CopyableMessageBox` class, `CopyableMessageBoxButtons` enumeration and `CopyableMessageBoxIcon` enumeration.

## s3pi.Controls

**When to include it:** When you want one of the custom controls.

**Summary:** This assembly provides custom controls related to data types in s3pi.

**Further reading:** `ResourceTypeCombo` class, `TGIBlockCombo` class and `TGIBlockListEditor` class. This area of the library still needs properly documenting.

## DDSPanel

**When to include it:** When you use DDS images and want a way to display them in a WinForms application.

**Summary:** This assembly provides a custom control and DDS resource support.

**Further reading:** `DDSPanel` class and `DDSPanel.MaskChannel` enumeration.

## s3pi.Helpers

**When to include it:** May be useful when writing a helper for s3pe.

**Summary:** Provides support for mapping a resource to one or more programs that may be interested in that kind of resource.

**Further reading:** `s3pi.Helpers` namespace. This area of the library still needs properly documenting.

# Setting Up Your Project

Set up the references you need based on the previous section `System.Custom`, `s3pi.Settings`, `s3pi.Interfaces` and any others. In addition, you will need to consider whether you want to reference specific wrapper assemblies. In most cases, this will be the appropriate approach. Your referenced assemblies will, by default, get copied to the project output folder, along with your program.

Note that there are certain additional components used, such as configuration files, that you will also need to arrange for your project's build to copy (if newer) to the output folder.

You may want to look at the s3oc and s3pe Visual Studio solutions to see how I've done it.

# Writing a wrapper

## Background

s3pi provides a number of C# classes to assist programs wanting to access Sims 3 package files and the resources stored within them. The "core library" only understands the package container itself -- it has no understanding of the content of resources. That is delegated to "wrappers". Wrappers are associated with resources by declaring the list of `ResourceTypes` they support. The core library returns an instance of the approriate resource class to the library "client".

So, wrappers have two main aims: to provide an "understanding" of the content of one or more resource types and to let the core library know which resource types they understand. A wrapper can provide one or more resource handlers as it sees fit but authors are encouraged to keep different concerns separated into different wrappers.

The core library identifies wrappers by searching through the assemblies in the s3pi library folder for those with the appropriate interface.

The `WrapperDealer` has an interface that lets client applications enable and disable particular combinations of `ResourceType` and wrapper.

## Assumptions

- You can code in C# (or another language that compiles to MSIL with code that the core library understands).
- You know the data format of the resource within a Sims 3 package and you want to write an extension to the s3pi library that allows the resource to be accessed by library "clients".
- You understood that last sentence ;).

## Pre-requisites

- Agree to the GPL3 licence before distributing your code. Notice that, as you're linking to a GPL3 library, you are essentially re-using it and must distribute your code under equivalent terms. You can find further details on <a href="http://www.fsf.org/">The Free Software Foundation's</a> site.
- VisualStudio or MONO (or...). I've been using VisualStudio 2012 Express Edition for Desktop and .Net 4. (I tested compiling the core under MONO and using it with the demo front end (as s3pe was known) compiled under VS2008 a long time ago. If this still works, it would be useful for someone to report success.)
- The s3pi library. You don't need the source (but it may be useful for examples... or not). You do need to install the library, though, and tell your development environment about it.

## Examples

### DefaultResource

I have now included `DefaultResource` in the s3pi library documentation.

The simplest example is that given in `DefaultResource` in the source distribution. It "does nothing" beyond what is essential for any wrapper.

It defines two classes:

```csharp
public class DefaultResource : AResource { }
public class DefaultResourceHandler : AResourceHandler { }
```

An assembly containing a wrapper must contain a class implementing `AResourceHandler`. This class provides an `IDictionary<Type, List<string>>` lookup between a class that implements `AResource` and a list of strings containing `ResourceType` values. (The strings used are the values from the `TypedValue` `ResourceType` cast to a string, i.e. a hex string.)

`DefaultResource` uses "*" to let `WrapperDealer` know it's happy to take everything. Do NOT use this in your wrappers!

`DefaultResource`, then.

It is strongly recommended you have `const Int32 recommendedApiVersion = 1;` at the top of your class for future compatibility. This does allow you to "version" your wrapper's API if you need it. Probably not that useful in practice, though.

You must have the property `AApiVersionedFields RecommendedApiVersion`.

The constructor for `DefaultResource` demonstrates an important point -- a new resource is created knowing **nothing**. It can check that it is new by checking that the stream passed to the constructor is null. It should then create a `MemoryStream` and populate it with the minimum valid data content for the resource.

That's it! There's nothing else you must do. Of course, you haven't provided anyone with a reason to use your wrapper yet...

### ImageResource

> [!NOTE]
> After some thought, `ImageResource` and `TextResource` are considered deprecated. However, they will remain in the library for the foreseeable future. Despite what is written below, I no longer consider it a good idea to have a wrapper for a resource that is just a byte stream.
>
> The approach taken for DDS resources is considered more correct. A corollory is directly stated below: the design here was influenced by s3pe rather than implementing anything regarding the structure of the data itself, which is the purpose of a wrapper. This should have been handled in a similar manner to DDS resources.
>
> A further corollory was the addition and then removal of the `_VID` resource wrapper during a QA cycle when it was discovered that the content was purely the output of a custom Electronic Arts audiovisual CODEC, rather than having any usefully decipherable content.
>
> In conclusion: "Value" should just be a string. Normally, the built in formatter builds an appropriate one from your public properties.

A slightly more complex example is ImageResource. It sticks to the two class model:

```csharp
public class ImageResource : AResource { }
public class ImageResourceHandler : AResourceHandler { }
```

This wrapper handles images - many different resource types are simply stored PNG89 files.

The static constructor for `ImageResourceHandler` reads a list of all the known image resource types (from a file in the folder where the assembly is located). This is then used to populate the list `IDictionary<Type, List<string>>` list in the instance constructor, which is then used by `WrapperDealer`. This means it's easy to add new Resource Types to the list this wrapper supports - just edit the text file and add them, no need to recompile. (It's such a good pattern, I should have made it easier to re-use...)

The `ImageResource` class remains fairly simple. Its constructor ensures there is a valid PNG89 image if passed a null stream. And it provides a single property called "Value" that returns a `System.Drawing.Image` created from the PNG89 data. It doesn't support saving images back to an existing resource.

The `Value` property is worth mentioning further -- the S3PI Demo Front End (now known as s3pe) checks whether a resource has a `Value` property and, if so, whether it has a Type of Image or String data. It knows how to display those two (and those two alone, currently). Returning the appropriate one can be useful in debugging.

You'll notice the stream position getting set to zero before use -- always assume it's in an unknown state.

The `TextResource` wrapper is very similar -- text resources are defined in a file; it has a "Value" property returning the resource as a string value. In addition, it has some text-specific properties, including accessing the data as XML. (It would be better design to have the XML wrapper as an extension of the text wrapper and only handle known XML files in it...)

### NameMapResource

This wrapper handles a single resource type - the package name map. It provides an `IDictionary<ulong, string>` interface, allowing the map to be read and updated. It's a very simple example of how to handle updates.

Here's how it works. This is a simple example but the pattern can be extended to more complex needs.

The work is done in a number of places:

The `Stream` property checks whether the resource has been dirtied (i.e. changed). If so, it discards the current stream and calls `UnParse()`.

The instance constructor checks for a null stream and calls `UnParse()` to construct the minimum valid resource.

The `Parse(Stream s)` method reads the data into the data structure used to manipulate it -- here a `Dictionary<ulong, string>`. As this is a repeating structure of varying length entries that allows data to be inserted, it's not efficient to use the data in the stream.

The `UnParse()` method exports the data structure back to a new stream, creating new objects where required.

As mentioned above, the `Stream` property needs to know if the resource has been dirtied. The implementation of the `IDictionary<ulong, string>` interface takes care of this by calling `OnResourceChanged(this, EventArgs.Empty)` for update operations. This is provided on `AResource` and sets the resource to dirty, as well as calling the `ResourceChanged` handlers of anything listening to the event.

## Self-Study

### CatalogResource

CatalogResource really just takes the ideas in NameMapResource further. It has an abstract class for a related set of resources. I've tried to keep to a consistent pattern of coding to help understand what's happening. I leave it as an exercise to the reader to work through the implementation to see how all the classes are interacting. Hopefully it should make sense! (If not, I'll be stumped when a bug comes up!!)

### RigResource

This has the benefit of being recent and, whilst relatively simple, does have some interesting bits. It is also one of the wrappers I most often consult myself when writing a new one.

# Writing an RCOL Block Handler

An RCOL resource is a normal resource, as described above - with the fundamental difference that it is a container for other "resources", known as RCOL blocks. Each block has a format identified by a four character code ("FOURCC" or Tag); the blocks also have Resource Types. The RCOL resource (in the package) has the same type as the first RCOL block in (in the resource) and the resource is named after this first RCOL block. Some RCOL resources only contain a single RCOL block; others contain multiple RCOL blocks.

The basic support is provided by `s3pi.GenericRCOLResource`. In addition to reading blocks from and writing blocks to the package, the resource wrapper has additional methods to support the RCOL format and there is a registry of RCOL block handlers.

(Note that the term "chunk" is loosely used to refer to an RCOL block at times...)

There is an abstract class, `ARCOLBlock`, that defines the fundamentals. There is a default implementation,  `DefaultRCOL`, for when no other matching RCOL block handler is defined in the registry, which provides minimal support.

Other than extending `ARCOLBlock` rather than `AResource`, the task of writing an RCOL block handler is very similar to writing a resource wrapper.

However, Electronic Arts / Maxis have recently started making life a little harder in that some single-RCOL containers do not exactly conform to the original understanding of how the container should work. Be aware of this when reading the code or writing your own.

# Tips To Avoid Package Files Conflicts

As you probably already understand, The Sims Package files are like a "zip file", containing several other files inside them. Each file/resource inside a Package file has a TGI associated with it.

The TGI is basically the Type, Group and Instance. The Type and Group are 8-digit Hexadecimal, while the Instance is a 16-digit Hexadecimal. In order to avoid conflicts between resources when loaded by the game, EVERY resource present in all Packages loaded by the game must have a unique TGI combination.

When you open a Package file with S3PE, you can easily see the Type, Group, and Instance of each resource present in the opened Package. Now that you know this, keep in mind that while editing The Sims Package files, you should always ensure that the resources you insert into Package files must always have a unique TGI.

If you have any further questions about this, you can read <a href="https://marcos4503.github.io/sims3-package-interface/Repository-Pages/other-content/about-mods-conflicts.pdf">this article</a>, which talks about and explains very well what mod conflicts are, how they occur, how to resolve them, etc.

# That's all!

If you've read this far, you should have a decent understanding of what S3PI is and how to use it. If you want to access the old official S3PI repository, you can use <a href="https://sourceforge.net/projects/s3pi/">this link</a>. Remembering that all credit for creating the S3PI library goes to Peter L Jones. 😀

<br>

<p align="center">
Repository created with ❤ by Marcos Tomaz
</p>