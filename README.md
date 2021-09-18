# Unshitfuckeryfier
An *uncompleted* automated fix for Bark/Trireme damage dealt/taken in RoN:EE. It should only be fed a balance.xml file that's been put through mjn33's [ron-objmask-workaround](https://github.com/mjn33/ron-objmask-workaround) utility.

After evaluating how long it would take to complete, I decided to manually change all the modifiers directly instead. This code could still be a useful starting point for doing something similar, particularly because it contains useful information about which units need what modifiers.

In its current state, all it does it read a unitrules.xml and balance.xml file, then outputs a list of units and their corresponding preq0 (which isually either an age, or "none" for Ancient Age units).
