Version 3.7 - 20 April 2018
---------------------------

* Added command priority. Priority allows different commands with the same label to take precedence in different situations.
* Revised constructors for `CommandAttribute` and `TriggerAttribute` to use named parameters.
* Commands in actions are no longer processed.

Version 3.6 – 2 August 2017
---------------------------

* Moved the IRC library to a separate repository.

Version 3.2 – 12 November 2015
------------------------------

* Done a lot of cleaning to the IRC library.
* Nickname, ident and full name are now supplied using an `IRCLocalUser` object.
* RPL_ISUPPORT extensions have been moved to the `Extensions` property.
* `IRCChannelUser.Status` is now a set of mode characters instead of an enum. It now works with any status flag mentioned in RPL_ISUPPORT. You can still compare like this: `channelUser.Status >= ChannelStatus.Halfop`.
* Some events have been rearranged.

Version 3.1.1 – 5 November 2014
-------------------------------

* The collection classes in the IRC library now use the correct comparison method as specified by the RPL_ISUPPORT.
* Most events that include a User object now use an object taken from the IRCClient.Users collection.
* **Fixed** the permission system, which was returning false positives.

Version 3.1 – 26 October 2014
-----------------------------

* All events in the IRC library, and in plugins, now use the standard EventHandler structure.
* Added the ability to log in using a server password.
* Bug fixes
* **Fix:** Permissions are now recognised correctly for channel access matches ($o).

Version 3.0.1 – 25 September 2014
---------------------------------

* Auto-joining channels is now done by a separate thread from the one receiving messages.

Version 3.0 – 24 September 2014 (The Great Rewrite)
---------------------------------------------------

* The entire project is being ported to C# and renamed accordingly to CBot.
* The IRC library has been redesigned. There is now a global list of users for each client, as well as a list of channels the bot is on, which each contain a list of users on the channel. Each list of users contains different information.
* The IRC library now supports gender codes.
* Plugins are now version locked. Old and incompatible plugins won't load.
* The minor channel concept has been retired. Plugins wishing to retain this functionality should maintain a list of channels on their own.
* Output attributes have been retired. The console now uses an IRCClient subclass to emulate an IRC channel. Other plugins can also do this.
* Command and Regex attributes now use an EventArgs subclass rather than a difficult-to-remember-and-change parameter list.
* Advanced Console Output is now known as CBot.ConsoleUtils, and its procedures have been rearranged. The escape character is now `%` instead of `\`.
