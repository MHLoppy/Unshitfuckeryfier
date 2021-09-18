# Unshitfuckeryfier
An *uncompleted* automated fix for Bark/Trireme damage dealt/taken in RoN:EE. It should only be fed a balance.xml file that's been put through mjn33's [ron-objmask-workaround](https://github.com/mjn33/ron-objmask-workaround) utility.

After evaluating how long it would take to complete, I decided to manually change all the modifiers directly instead. This code could still be a useful starting point for doing something similar, particularly because it contains useful information about which units need what modifiers.

In its current state, all it does it read a unitrules.xml and balance.xml file, then outputs a list of units and their corresponding preq0 (which isually either an age, or "none" for Ancient Age units).

### The original Bark/Trireme bug
In RoN:EE (and not previous versions of the game), not only are object masks bugged, but for some ungodly reason Barks and Triremes are internally hardcoded as Modern Age (VII) units, meaning they deal far too much damage to low-age units, and receive far too little damage from high-age units. They perform equivalently to Medieval Age (III) units in this bugged state which can be hugely problematic balance-wise on maps involving naval combat. Additionally, elephants of all ages (because of course) also seem to have an unexpected -30% damage penalty vs Barks and Triremes in RoN:EE.

### The fix for that original bug
This can be approximately resolved by adding modifiers for/against units based on Age. For example, Barks and Triremes can be set to have a 59% damage modifier vs Ancient Age (I) units.

### The second bug
When balance.xml is loaded from a local mod, the game appears to apply modifiers for Barks and Triremes correctly for the first game played, but _twice_ for all games played after that. The result is Triremes and Barks being as or more bugged than before, just in the opposite direction.

### The second fix
Thinking this was a bug with the age-related modifiers in balance.xml, I wanted to fold those modifiers directly into the unit modifiers themselves (much like what ron-objmask-workaround does with object masks), hence Unshitfuckeryfier. Unfortunately it appears that the problem is *specifically* the modifiers for Barks and Triremes which are being incorrectly loaded rather than the age-related modifiers (which I should've been able to notice before that, but alas I didn't).

### The third fix
In the end, the solution is to **directly modify or replace** the game's files rather than implement changes at the local-mod level, although a completed version of Unshitfuckeryfier might still be able to be used in conjunction with that solution.
