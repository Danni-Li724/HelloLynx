//VARIABLES AND EXTERNAL REFERENCES //
VAR collisionControl_Victory = false
VAR byteRunner_Victory = false

EXTERNAL ExitDialogue()
EXTERNAL StartCollisionControl()
EXTERNAL StartByteRunner()
EXTERNAL AwardCollisionControlBadge()
EXTERNAL AwardByteRunnerBadge()
EXTERNAL AwardScreenBadge()
EXTERNAL AwardRamBadge()
EXTERNAL AwardAudioBadge()
EXTERNAL AwardPowerBadge()

/////////////////////////////////////

=== suzyDialogue ===
    = FirstIntroduction
            Oh wow! Someone actually showed up! # speaker: Suzy
            Hey there—! I'm Suzy. I'm in charge of everything you see on screen. Sprites, colors, movement, pixel collisions... I'm your one chip animation studio! # speaker: Suzy
            I operate at 16 MHz - four times faster than Mikey. Not that I'm bragging...Okay, maybe a little~ # speaker: Suzy
            But enough about me! What brings you to this old motherboard? # speaker: Suzy
            + [> Honestly, this whole thing fascinates me.] # speaker: Player
                Love that. Not many people take the time to notice what goes on inside their machines! # speaker: Suzy
                -> Questions
            + [> I just wanted to see how games worked back then.] # speaker: Player
                Retro detective, huh? You’re in the right place. # speaker: Suzy
                -> Questions
            + [> I like learning weird stuff.] # speaker: Player
                Weird is underrated. You've got a good brain. I like you already ;)! # speaker: Suzy
            You ask the right kind of questions. Curious, grounded... you'd make a decent engineer. Or artist. Or both! Want to know how I work? I promise it’s cooler than it sounds. # speaker: Suzy
            -> Questions
    
    = ReturningIntroduction
        Hey welcome back! # speaker: Suzy
            + [> Could you explain your role here again?] # speaker: Player
                Sure! # speaker: Suzy
                -> Questions
            + [> Mind if I have another go at 'Collision Control'?] # speaker: Player
                Love your enthusiasm! Need a quick refresher? # speaker: Suzy
                    ++ [> Yes.] # speaker: Player
                        -> CollisionControlInstructions    
                    ++ [> Nope, I'm ready!] # speaker: Player
                        ~ StartCollisionControl()
                        -> DONE
            
    -> Questions
    = Questions
        //CHOICE A (#0)
        + [> How do you draw sprites?] # speaker: Player
            Okay, picture this: You want a spaceship zooming across the screen. You don't just throw me an image and say "go". I need two things: # speaker: Suzy
            1. Pixel data: the raw visual. # speaker: Suzy
            2. A Sprite Control Block: basically a set of instructions telling me how to handle it. # speaker: Suzy
            Give me those, and I'll blit that thing onto the screen buffer in no time. # speaker: Suzy
            -> Questions
        
        //CHOICE B (#1)
        + [> How does collision detection work?] # speaker: Player
            Ah, my secret sauce! I have a technique that no other consoles at my time have figured out. # speaker: Suzy
            While I'm drawing, I also keep an eye out for overlaps. Each sprite gets a collision number - sort of like a name tag, numbered 0 to 15. # speaker: Suzy
            If two sprites overlap, I record the highest number into something called the collision depository. You (or your game code) can check it afterward to see what hit what. And yes - this is pixel-perfect. None of that rectangle-guessing nonsense other consoles are used to. # speaker: Suzy
            -> Questions
        
        //CHOICE C (#2)
        + [> Do you have any unique tricks for processing graphics?] # speaker: Player
            Absolutely! I'm actually really well known for reusing graphics to be more efficient, I'm super proud of it. # speaker: Suzy
            One piece of pixel data, let's say a robot, can be reused in multiple SCBs. I can draw it in different spots, scales, or colours without duplicating the actual image data. Saves memory and still looks great, Win win! # speaker: Suzy
            -> Questions
        
        //CHOICE D (#3)
        + [> Any chance I could see you in action?] # speaker: Player
            I can do you one better! I'll let you try your hand at 'Collision Control'! # speaker: Suzy
            -> CollisionControlInstructions
        
    = CollisionControlInstructions
        Just press the SPACE BAR whenever you see two objects collide on-screen.
        Sounds simple but its quite tough! You sure you're ready? 
            + [> Sure am!] # speaker: Player
                I love your confidence, Let's get started! # speaker: Suzy
                ~ StartCollisionControl()
                -> DONE
            + [> Not yet...] # speaker: Player
                Oh, that's okay. Come back soon! # speaker: Suzy
                In the meantime, make sure you meet Mikey if you haven't already. # speaker: Suzy
                ~ ExitDialogue()
                -> DONE
    
    = PostMinigame
        {collisionControl_Victory:
            Wow, you did great! # speaker: Suzy
            That task can get really hectic at times but you held your own pretty well. # speaker: Suzy
            Here, take this 'Collision Control' badge as a token of my appreciation! # speaker: Suzy
            [YOU TAKE THE BADGE] #speaker: Player
            Thanks for all your help! # speaker: Suzy
            ~ ExitDialogue()
            -> DONE
            
        - else:
            Tough isn't it? Would you like to try again? # speaker: Suzy
                + [> Yes.] # speaker: Player
                    Good luck! # speaker: Suzy
                    ~ StartCollisionControl()
                    -> DONE
                + [> No.] # speaker: Player
                    Feel free to come back and try again whenever you'd like. # speaker: Suzy
                   ~ ExitDialogue()
                    -> DONE
        }
        
        

=== mikeyDialogue ===
    = FirstIntroduction
    Hi there. (He adjusts his glasses, looking up from his clipboard.) # speaker: Mikey
            You don't look like you're from around here. Curious visitor, huh? # speaker: Mikey
            I'm Mikey, the main processor of the Lynx. That said, I wouldn't get very far without Suzy. She's faster, louder... and never lets me forget it. # speaker: Mikey
            But I manage the system’s brain: logic, inputs, audio, and timing. I'm a bit more methodical - I keep things running smoothly at 4 MHz, one cycle at a time. # speaker: Mikey
            Say, how's your day going so far? # speaker: Mikey
            + [> Honestly? Kinda great. I’ve never seen anything like this.] # speaker: Player
                That's good to hear. Always nice when someone shows up with an open mind. # speaker: Mikey
                -> Questions
            + [> It's cool, kinda surreal. Everything’s so... alive in here.] # speaker: Player
                I’m glad you feel that way. There's a lot going on under the surface—literally. # speaker: Mikey
                -> Questions
            + [> I’m just happy to learn something new.] # speaker: Player
                That’s an excellent mindset. Curiosity is how you make the invisible visible. # speaker: Mikey
                -> Questions
            Honestly? My days are always productive. Busy, but satisfying. That’s the joy of structure, you always know what comes next. # speaker: Mikey
            Anyway, if you’re interested, I’ve got time for some questions. Or I can show you what I’ve been working on. # speaker: Mikey
            -> Questions
        
    = ReturningIntroduction
        Oh, you're back. Wanna give me a hand? # speaker : Mikey
            + [> Could I ask you a question?] # speaker : Player
                Sure, fire it away. # speaker: Mikey
                -> Questions
            + [> Yeah, what do you need me to do?] #speaker : Player
              We're doing 'Byte Runner', do you still remember it? # speaker : Mikey
                ++ [> No, could you remind me?] # speaker : Player
                    -> ByteRunnerInstructions
                ++ > I do! I've got this. # speaker: Player
                    ~ StartByteRunner()
                    -> DONE
    
    = Questions
        //CHOICE A (#0)
        + [> How does an 8-bit CPU like you work?] # speaker = Player
            I'm based on the 6502 family of processors. Classic architecture. I handle 8 bits of data at a time; meaning I process numbers from 0 to 255 in a single operation. # speaker: Mikey
            Larger numbers? More complex math? I break them down and handle them step by step. # speaker: Mikey
            It's slower, sure, but it works. No floating point, no shortcuts. Just pure byte by byte logic. # speaker: Mikey
            I operate within a 16bit address space, which gives me 64 kilobytes of memory. That's the total memory I can access at once; for code, variables, assets, everything. # speaker: Mikey
            It sounds small by today's standards, but with proper structure and clever loading, it's enough. And we were the pioneers of our time. Suzy, for instance, knows how to reuse graphics. I myself also admire that efficiency. # speaker: Mikey
            -> Questions
        
        //CHOICE B (#1)
        + [> What kind of sound can you make?] # speaker = Player
            I control a 4 channel sound system. Each channel plays its own waveform: like square waves, sawtooth, or noise. # speaker: Mikey
            You set the pitch, volume, and length, and I handle the playback. I can even mix in digital samples, though that's more demanding. I'm no synthesizer, but I do a good job delivering clean, retro audio. # speaker: Mikey
            -> Questions
            
        //CHOICE C (#2)
        + [> What else can you do?] # speaker = Player
            I work closely with Suzy, handling video timing - starting the screen refresh cycle, managing blanking intervals, and making sure Suzy doesn't draw in the middle of a frame. # speaker: Mikey
            If you've ever seen screen tearing in a game, that's what happens when graphics and timing go out of sync. I make sure that never happens here. # speaker: Mikey
            Although I handle a lot of tasks, handling anything more than one task at a time isn't my thing especially not how modern systems do. Don't get me wrong though I do move fast, so it feels smooth. # speaker: Mikey
            I run a main loop, one task at a time: check input, update logic, refresh screen, repeat. # speaker: Mikey
            -> Questions
            
        //CHOICE D (#3)
        + [> Anything I can help you with?] # speaker = Player
            Sure, it would be nice to have another helping hand around here. Would you like to try and allocate some bytes for me, experience for yourself how an 8-bit processor works? Join me in 'Byte Runner'! # speaker: Mikey
            -> ByteRunnerInstructions
            
    = ByteRunnerInstructions
        Don't worry it's simple, I'll send you address signals down the bus - you read 'em, and drag my byte workers into the right memory slots before Suzy hogs the line again! # speaker: Mikey
            + [> I'm ready!] # speaker: Player
                That's what I like to hear! # speaker: Mikey
                ~ StartByteRunner()
                -> DONE
            + [> I need more time...] # speaker: Player
                No stress, come back whenever you're ready. # speaker : Mikey
                ~ ExitDialogue()
                -> DONE
                
=== screenTrivia ===
        = Introduction
            This is the Lynx's screen. What you’re looking at is a TFT LCD with double buffering. # speaker: Narrator
            Double buffering means Suzy can draw the next frame in memory while the current one is still being shown—so the screen never flickers. # speaker: Narrator
            It’s a technique still used in graphics today. # speaker: Narrator
            Ready for a quick question? # speaker: Narrator
                + [> Sure!] # speaker: Player
                    -> Quiz
                + [> Maybe later.] # speaker: Player
                    ~ ExitDialogue()
                    -> DONE
    
        = Quiz
            What does “double buffering” help prevent? # speaker: Narrator
                + [> Screen tearing] # correct
                    That’s right! When graphics and screen refreshes aren’t in sync, you get tearing. Double buffering avoids that. # speaker: Narrator
                    ~ AwardScreenBadge()
                    ~ ExitDialogue()
                    -> DONE
                + [> Memory leaks]
                    Not quite. Memory leaks are a different kind of problem. # speaker: Narrator
                    ~ ExitDialogue()
                    -> DONE
                + [> Low battery]
                    Nah, that’s a power issue, not a graphics one. But fair guess. # speaker: Narrator
                    ~ ExitDialogue()
                    -> DONE
    
=== ramTrivia ===
        = Introduction
            This is the Lynx's main RAM, which contains 64 kilobytes of fast access memory. # speaker: Narrator
            64k...that sounds like a lot, right? But wait, modern computers have 32,000,000 KB! # speaker: Narrator
            Everything: game code, graphics, sound...must fit into this space at runtime. # speaker: Narrator
            It’s not much, but used cleverly, it’s enough to run entire worlds. # speaker: Narrator
            Want to test your memory on memory? # speaker: Narrator
                + [> Let’s go!] # speaker: Player
                    -> Quiz
    
        = Quiz
            How much RAM does the Lynx have? # speaker: Narrator
                + [> 64 kilobytes] # correct
                    You got it! That’s 65,536 bytes: tight quarters, but doable. # speaker: Narrator
                    ~ AwardRamBadge()
                    ~ ExitDialogue()
                    -> DONE
                + [> 1 megabyte]
                    Too generous. This isn’t a modern system. # speaker: Narrator
                    ~ ExitDialogue()
                    -> DONE
                + [> 128 kilobytes]
                    Close, but nope! # speaker: Narrator
                    ~ ExitDialogue()
                    -> DONE

=== speakerTrivia ===
        = Introduction
            Tiny but mighty. The Lynx speaker can play four separate sound channels at once. # speaker: Narrator
            Each channel plays a waveform: square, sawtooth, or even noise. Mikey mixes them in real time. # speaker: Narrator
            Care to put your ears to the test? # speaker: Narrator
                + [> Hit me with it.] # speaker: Player
                    -> Quiz
    
        = Quiz
            How many audio channels can the Lynx play at once? # speaker: Narrator
                + [> 4] # correct
                    Yep! Four channels, handled entirely in hardware. # speaker: Narrator
                    ~ AwardAudioBadge()
                    ~ ExitDialogue()
                    -> DONE
                + [> 2]
                    Not quite—this isn’t mono! # speaker: Narrator
                    ~ ExitDialogue()
                    -> DONE
                + [> 6]
                    That’d be overkill for 1989. # speaker: Narrator
                    ~ ExitDialogue()
                    -> DONE

=== powerTrivia ===
    = Introduction
        This switch routes power to the LCD backlight circuit: the light behind the screen that lets you play in the dark. # speaker: Narrator
        On old handhelds, the backlight is one of the hungriest parts. Flip this on, and a DC/DC regulator boosts and smooths the voltage so the lamp gets clean power without flicker. # speaker: Narrator
        Meanwhile, the main board still has to power Mikey and Suzy at stable logic levels so the Lynx uses regulation and capacitors to keep the rails steady when the backlight kicks in. # speaker: Narrator
        Wanna do a quick power quiz? # speaker: Narrator
            + [> Light me up.] # speaker: Player
                -> Quiz
            + [> Maybe later.] # speaker: Player
                ~ ExitDialogue()
                -> DONE

    = Quiz
        Why does turning on the LCD backlight drain the battery faster on retro handhelds? # speaker: Narrator
            + [> The backlight draws significant current, so the regulator has to supply more power.] # correct
                Exactly. The backlight is a big load; more current out means the batteries empty sooner. Good regulation keeps the rest of the system stable. # speaker: Narrator
                ~ AwardPowerBadge()
                ~ ExitDialogue()
                -> DONE
            + [> The CPU underclocks itself and wastes energy as heat.]
                Not quite. Underclocking usually *reduces* draw; the real hog here is the lamp’s current. # speaker: Narrator
                ~ ExitDialogue()
                -> DONE
            + [> The screen turns black, so the pixels consume extra power.]
                Nope. The backlight behind the LCD is the main power consumer, not the dark pixels. # speaker: Narrator
                ~ ExitDialogue()
                -> DONE
                            
    -> DONE
    
