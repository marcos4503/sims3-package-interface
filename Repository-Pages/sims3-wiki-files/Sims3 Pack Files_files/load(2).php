function isCompatible(){if(navigator.appVersion.indexOf('MSIE')!==-1&&parseFloat(navigator.appVersion.split('MSIE')[1])<6){return false;}return true;}var startUp=function(){mw.config=new mw.Map(true);mw.loader.addSource({"local":{"loadScript":"/load.php","apiScript":"/api.php"}});mw.loader.register([["site","1676387504",[],"site"],["noscript","1676387504",[],"noscript"],["startup","1707059925",[],"startup"],["filepage","1676387504"],["user.groups","1676387504",[],"user"],["user","1676387504",[],"user"],["user.cssprefs","1707059925",["mediawiki.user"],"private"],["user.options","1707059925",[],"private"],["user.tokens","1676387504",[],"private"],["mediawiki.language.data","1707038150",["mediawiki.language.init"]],["skins.chick","1676387504"],["skins.cologneblue","1676387504"],["skins.modern","1676387504"],["skins.monobook","1676387504"],["skins.nostalgia","1676387504"],["skins.simple","1676387504"],["skins.standard","1676387504"],["skins.vector","1676387504"],["jquery","1676387504"],[
"jquery.appear","1676387504"],["jquery.arrowSteps","1676387504"],["jquery.async","1676387504"],["jquery.autoEllipsis","1676387504",["jquery.highlightText"]],["jquery.badge","1676387504"],["jquery.byteLength","1676387504"],["jquery.byteLimit","1676387504",["jquery.byteLength"]],["jquery.checkboxShiftClick","1676387504"],["jquery.client","1676387504"],["jquery.collapsibleTabs","1676387504"],["jquery.color","1676387504",["jquery.colorUtil"]],["jquery.colorUtil","1676387504"],["jquery.cookie","1676387504"],["jquery.delayedBind","1676387504"],["jquery.expandableField","1676387504",["jquery.delayedBind"]],["jquery.farbtastic","1676387504",["jquery.colorUtil"]],["jquery.footHovzer","1676387504"],["jquery.form","1676387504"],["jquery.getAttrs","1676387504"],["jquery.highlightText","1676387504",["jquery.mwExtension"]],["jquery.hoverIntent","1676387504"],["jquery.json","1676387504"],["jquery.localize","1676387504"],["jquery.makeCollapsible","1678132544"],["jquery.mockjax","1676387504"],[
"jquery.mw-jump","1676387504"],["jquery.mwExtension","1676387504"],["jquery.placeholder","1676387504"],["jquery.qunit","1676387504"],["jquery.qunit.completenessTest","1676387504",["jquery.qunit"]],["jquery.spinner","1676387504"],["jquery.jStorage","1676387504",["jquery.json"]],["jquery.suggestions","1676387504",["jquery.autoEllipsis"]],["jquery.tabIndex","1676387504"],["jquery.tablesorter","1678982495",["jquery.mwExtension"]],["jquery.textSelection","1676387504",["jquery.client"]],["jquery.validate","1676387504"],["jquery.xmldom","1676387504"],["jquery.tipsy","1676387504"],["jquery.ui.core","1676387504",["jquery"],"jquery.ui"],["jquery.ui.widget","1676387504",[],"jquery.ui"],["jquery.ui.mouse","1676387504",["jquery.ui.widget"],"jquery.ui"],["jquery.ui.position","1676387504",[],"jquery.ui"],["jquery.ui.draggable","1676387504",["jquery.ui.core","jquery.ui.mouse","jquery.ui.widget"],"jquery.ui"],["jquery.ui.droppable","1676387504",["jquery.ui.core","jquery.ui.mouse","jquery.ui.widget",
"jquery.ui.draggable"],"jquery.ui"],["jquery.ui.resizable","1676387504",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.selectable","1676387504",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.sortable","1676387504",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.accordion","1676387504",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.ui.autocomplete","1676387504",["jquery.ui.core","jquery.ui.widget","jquery.ui.position"],"jquery.ui"],["jquery.ui.button","1676387504",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.ui.datepicker","1676387504",["jquery.ui.core"],"jquery.ui"],["jquery.ui.dialog","1676387504",["jquery.ui.core","jquery.ui.widget","jquery.ui.button","jquery.ui.draggable","jquery.ui.mouse","jquery.ui.position","jquery.ui.resizable"],"jquery.ui"],["jquery.ui.progressbar","1676387504",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.ui.slider",
"1676387504",["jquery.ui.core","jquery.ui.widget","jquery.ui.mouse"],"jquery.ui"],["jquery.ui.tabs","1676387504",["jquery.ui.core","jquery.ui.widget"],"jquery.ui"],["jquery.effects.core","1676387504",["jquery"],"jquery.ui"],["jquery.effects.blind","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.bounce","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.clip","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.drop","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.explode","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.fade","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.fold","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.highlight","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.pulsate","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.scale","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.shake",
"1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.slide","1676387504",["jquery.effects.core"],"jquery.ui"],["jquery.effects.transfer","1676387504",["jquery.effects.core"],"jquery.ui"],["mediawiki","1676387504"],["mediawiki.api","1676387504",["mediawiki.util"]],["mediawiki.api.category","1676387504",["mediawiki.api","mediawiki.Title"]],["mediawiki.api.edit","1676387504",["mediawiki.api","mediawiki.Title"]],["mediawiki.api.parse","1676387504",["mediawiki.api"]],["mediawiki.api.titleblacklist","1676387504",["mediawiki.api","mediawiki.Title"]],["mediawiki.api.watch","1676387504",["mediawiki.api","user.tokens"]],["mediawiki.debug","1676387504",["jquery.footHovzer"]],["mediawiki.debug.init","1676387504",["mediawiki.debug"]],["mediawiki.feedback","1676387504",["mediawiki.api.edit","mediawiki.Title","mediawiki.jqueryMsg","jquery.ui.dialog"]],["mediawiki.htmlform","1676387504"],["mediawiki.notification","1676387504",["mediawiki.page.startup"]],["mediawiki.notify","1676387504"],
["mediawiki.searchSuggest","1678132544",["jquery.autoEllipsis","jquery.client","jquery.placeholder","jquery.suggestions"]],["mediawiki.Title","1676387504",["mediawiki.util"]],["mediawiki.Uri","1676387504"],["mediawiki.user","1676387504",["jquery.cookie","mediawiki.api"]],["mediawiki.util","1678132209",["jquery.client","jquery.cookie","jquery.mwExtension","mediawiki.notify"]],["mediawiki.action.edit","1676387504",["jquery.textSelection","jquery.byteLimit"]],["mediawiki.action.edit.preview","1676387504",["jquery.form","jquery.spinner"]],["mediawiki.action.history","1676387504",[],"mediawiki.action.history"],["mediawiki.action.history.diff","1676387504",[],"mediawiki.action.history"],["mediawiki.action.view.dblClickEdit","1676387504",["mediawiki.util"]],["mediawiki.action.view.metadata","1678153906"],["mediawiki.action.view.rightClickEdit","1676387504"],["mediawiki.action.watch.ajax","1676387504",["mediawiki.page.watch.ajax"]],["mediawiki.language","1676387504",["mediawiki.language.data",
"mediawiki.cldr"]],["mediawiki.cldr","1676387504",["mediawiki.libs.pluralruleparser"]],["mediawiki.libs.pluralruleparser","1676387504"],["mediawiki.language.init","1676387504"],["mediawiki.jqueryMsg","1676387504",["mediawiki.util","mediawiki.language"]],["mediawiki.libs.jpegmeta","1676387504"],["mediawiki.page.ready","1676387504",["jquery.checkboxShiftClick","jquery.makeCollapsible","jquery.placeholder","jquery.mw-jump","mediawiki.util"]],["mediawiki.page.startup","1676387504",["jquery.client","mediawiki.util"]],["mediawiki.page.watch.ajax","1691650296",["mediawiki.page.startup","mediawiki.api.watch","mediawiki.util","mediawiki.notify","jquery.mwExtension"]],["mediawiki.special","1676387504"],["mediawiki.special.block","1676387504",["mediawiki.util"]],["mediawiki.special.changeemail","1687816941",["mediawiki.util"]],["mediawiki.special.changeslist","1676387504",["jquery.makeCollapsible"]],["mediawiki.special.movePage","1676387504",["jquery.byteLimit"]],["mediawiki.special.preferences",
"1676387504"],["mediawiki.special.recentchanges","1676387504",["mediawiki.special"]],["mediawiki.special.search","1678137128"],["mediawiki.special.undelete","1676387504"],["mediawiki.special.upload","1691650330",["mediawiki.libs.jpegmeta","mediawiki.util"]],["mediawiki.special.javaScriptTest","1676387504",["jquery.qunit"]],["mediawiki.tests.qunit.testrunner","1676387504",["jquery.qunit","jquery.qunit.completenessTest","mediawiki.page.startup","mediawiki.page.ready"]],["mediawiki.legacy.ajax","1676387504",["mediawiki.util","mediawiki.legacy.wikibits"]],["mediawiki.legacy.commonPrint","1676387504"],["mediawiki.legacy.config","1676387504",["mediawiki.legacy.wikibits"]],["mediawiki.legacy.IEFixes","1676387504",["mediawiki.legacy.wikibits"]],["mediawiki.legacy.protect","1676387504",["mediawiki.legacy.wikibits","jquery.byteLimit"]],["mediawiki.legacy.shared","1676387504"],["mediawiki.legacy.oldshared","1676387504"],["mediawiki.legacy.upload","1676387504",["mediawiki.legacy.wikibits",
"mediawiki.util"]],["mediawiki.legacy.wikibits","1676387504",["mediawiki.util"]],["mediawiki.legacy.wikiprintable","1676387504"],["ext.nuke","1676387504"]]);mw.config.set({"wgLoadScript":"/load.php","debug":false,"skin":"vector","stylepath":"http://www.simswiki.info/skins","wgUrlProtocols":"http\\:\\/\\/|https\\:\\/\\/|ftp\\:\\/\\/|irc\\:\\/\\/|ircs\\:\\/\\/|gopher\\:\\/\\/|telnet\\:\\/\\/|nntp\\:\\/\\/|worldwind\\:\\/\\/|mailto\\:|news\\:|svn\\:\\/\\/|git\\:\\/\\/|mms\\:\\/\\/|\\/\\/","wgArticlePath":"/wiki.php?title=$1","wgScriptPath":"","wgScriptExtension":".php","wgScript":"/index.php","wgVariantArticlePath":false,"wgActionPaths":{},"wgServer":"http://direct.simswiki.info","wgUserLanguage":"en","wgContentLanguage":"en","wgVersion":"1.20.2","wgEnableAPI":true,"wgEnableWriteAPI":true,"wgMainPageTitle":"Main Page","wgFormattedNamespaces":{"-2":"Media","-1":"Special","0":"","1":"Talk","2":"User","3":"User talk","4":"SimsWiki","5":"SimsWiki talk","6":"File","7":"File talk","8":
"MediaWiki","9":"MediaWiki talk","10":"Template","11":"Template talk","12":"Help","13":"Help talk","14":"Category","15":"Category talk","100":"Content List","101":"Content List Talk","110":"Tutorials","111":"Tutorials Talk","120":"Stories","121":"Stories Talk","130":"MTS2","131":"MTS2 Talk","140":"Spore","141":"Spore Talk","150":"Sims 3","151":"Sims 3 Talk","160":"Sims 3 Content List","161":"Sims 3 Content List Talk","170":"MTS","171":"MTS Talk","180":"Game Help","181":"Game Help Talk","190":"Sims 4","191":"Sims 4 Talk"},"wgNamespaceIds":{"media":-2,"special":-1,"":0,"talk":1,"user":2,"user_talk":3,"simswiki":4,"simswiki_talk":5,"file":6,"file_talk":7,"mediawiki":8,"mediawiki_talk":9,"template":10,"template_talk":11,"help":12,"help_talk":13,"category":14,"category_talk":15,"content_list":100,"content_list_talk":101,"tutorials":110,"tutorials_talk":111,"stories":120,"stories_talk":121,"mts2":130,"mts2_talk":131,"spore":140,"spore_talk":141,"sims_3":150,"sims_3_talk":151,
"sims_3_content_list":160,"sims_3_content_list_talk":161,"mts":170,"mts_talk":171,"game_help":180,"game_help_talk":181,"sims_4":190,"sims_4_talk":191,"image":6,"image_talk":7,"project":4,"project_talk":5},"wgSiteName":"SimsWiki","wgFileExtensions":["png","gif","jpg","jpeg"],"wgDBname":"simswiki","wgFileCanRotate":true,"wgAvailableSkins":{"cologneblue":"CologneBlue","vbgore":"vbGORE","monobook":"MonoBook","chick":"Chick","standard":"Standard","vector":"Vector","simple":"Simple","vbgoreclean":"vbGOREClean","nostalgia":"Nostalgia","modern":"Modern","myskin":"MySkin","mts2":"MTS2"},"wgExtensionAssetsPath":"/extensions","wgCookiePrefix":"simswiki","wgResourceLoaderMaxQueryLength":-1,"wgCaseSensitiveNamespaces":[]});};if(isCompatible()){document.write("\x3cscript src=\"/load.php?debug=false\x26amp;lang=en\x26amp;modules=jquery%2Cmediawiki\x26amp;only=scripts\x26amp;skin=vector\x26amp;version=20150609T081956Z\"\x3e\x3c/script\x3e");}delete isCompatible;
/* cache key: simswiki:resourceloader:filter:minify-js:7:3bf719541bf7f2897755232f37f9bf1d */
/* Cached 20240204151845 */