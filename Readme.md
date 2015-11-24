# Associativy Internal Link Graph Builder Readme



## Project Description

Orchard module for automatically creating Associativy graphs (http://associativy.com/) from internal links.


## Features

- Automatically connect content items if one links to the other
- Admin UI configuration


## Documentation

This module adds the ability to automatically create [Associativy graphs](http://associativy.com/) from internal links between content items.

**This module depends on [Associativy](http://associativy.com/). Please install it first!**

The modules adds a checkbox to the graph management pages for each graph. If checked, if a content item that has the BodyPart attached changes from that graph then internal links in the html will be taken into account to add connections to the graph (i.e. if a content item that's in the graph is linked from another one then those two items will get connected).

If you want to have an implementation for something else then body text then just write your own handler and call IInternalLinksExtractor.ProcessHtml().

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/associativy-internal-link-graph-builder](https://bitbucket.org/Lombiq/associativy-internal-link-graph-builder) (Mercurial repository)
- [https://github.com/Lombiq/Associativy-Internal-Link-Graph-Builder](https://github.com/Lombiq/Associativy-Internal-Link-Graph-Builder) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.