=== mikeyIntroduction ===
//Mikey's dialogue for meeting the player for the first time

What do you want?
Oh, you must be the new guy.
Sorry for the attitude... Things have been pretty crazy around here!
Anyways, i'm Micheal. But everyone around here just calls me Mikey.
My job isn't easy but its vital for the operation of the Lynx.
The controls, soundtracks, timers, aswell as anything that moves, clicks or counts is my problem.
Sounds like a lot to handle but i've got what it takes.
Not to imply that i'm a one-man team though.
I share RAM with Suzy and she always gets priority for those flashy visuals.
In fact, I can't function without her!
Sorry but i'm going to have to cut our introduction short, i've got quite a bit to do.
Y'know I could actually use your help with a short task if you're willing to give it a go.
Its called 'Byte Runner'. 
I'll send you address signals down the bus, you read them, and drag my byte workers into the right memory slots before Suzy hogs the line again!
We'll have to be fairly quick once we start so make sure you're ready and then we can begin!

* [I'm ready!]
-> mikeAccept

* [I need more time...]
-> mikeDecline

=== mikeAccept ===
Give it all you got!
//GO TO 'BYTE RUNNER' GAME
-> END

=== mikeDecline ===
That's fine, i'll be here.
//EXIT DIALOGUE
-> END

=== suzyIntroduction ===
//Suzy's dialogue for meeting the player for the first time

Hiya! The name's Suzy.
I'm the reason the Lynx looks so fabulous!
Thats right, i'm not just here for my good looks!
Have you met Mikey yet? That genius and I make a pretty good team!
While he does all the number crunching, I'm scaling sprites, flipping them midair, and slapping pixels into place.
All in real time!
Other handhelds had to FAKE what I do.
I'm talking boring tiles, rigid sprites, and no transformations.
But not me!
I've got a 16-bit math co-processor tucked under these shoulder pads!
Gee someone like me should know that showing is better than telling.
How 'bout you join me in 'Collision Control'?
Just press the Spacebar whenever you see two objects collide on-screen.
Sounds simple but it's quite tough! You sure you're ready?

* [Sure am!]
-> suzyAccept

* [I need more time...]
-> suzyDecline

=== suzyAccept ===
Wow you're confident, I like it!
//GO TO 'COLLISION CONTROL' GAME
-> END

=== suzyDecline ===
Oh, okay. Come back soon!
-> END
