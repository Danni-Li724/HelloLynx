//VARIABLES AND EXTERNAL REFERENCES //
VAR collisionControl_Victory = false
VAR byteRunner_Victory = false

EXTERNAL ExitDialogue()
EXTERNAL StartCollisionControl()
EXTERNAL StartByteRunner()
EXTERNAL AwardCollisionControlBadge()
EXTERNAL AwardByteRunnerBadge()

/////////////////////////////////////

=== suzyDialogue ===
    = FirstIntroduction
        Oh hey, this is rare. Looks like I got a visitor today! # speaker: Suzy
        I'm Suzy. I'm in charge of everything graphical you see on screen: Sprites, colours, collisions, movement... # speaker: Suzy
        To handle them I have to move fast. 16MHz fast, actually! That's four times faster than Mikey, which comes in handy when you're pushing pixels like I do! # speaker: Suzy
        -> Questions
    
    = ReturningIntroduction
        Hey welcome back! # speaker: Suzy
            + > Could you explain your role here again? # speaker: Player
                Sure! # speaker: Suzy
                -> Questions
            + > Mind if I have another go at 'Collision Control'? # speaker: Player
                Love your enthusiasm! Need a quick refresher? # speaker: Suzy
                    ++ > Yes. # speaker: Player
                        -> CollisionControlInstructions    
                    ++ > Nope, I'm ready! # speaker: Player
                        ~ StartCollisionControl()
                        -> DONE
            
    -> Questions
    = Questions
        //CHOICE A (#0)
        + > How do you draw sprites? # speaker: Player
            Okay, picture this: You want a spaceship zooming across the screen. You don't just throw me an image and say "go". I need two things: # speaker: Suzy
            1. Pixel data - the raw visual. # speaker: Suzy
            2. A Sprite Control Block - basically a set of instructions telling me how to handle it. # speaker: Suzy
            Give me those, and I'll blit that thing onto the screen buffer in no time. # speaker: Suzy
            -> Questions
        
        //CHOICE B (#1)
        + > How does collision detection work? # speaker: Player
            Ah, my secret sauce! I have a technique that no other consoles at my time have figured out. # speaker: Suzy
            While I'm drawing, I also keep an eye out for overlaps. Each sprite gets a collision number - sort of like a nametag, numbered 0 to 15. # speaker: Suzy
            If two sprites overlap, I record the highest number into something called the collision depository. You (or your game code) can check it afterward to see what hit what. And yes - this is pixel-perfect. None of that rectangle-guessing nonsense other consoles are used to. # speaker: Suzy
            -> Questions
        
        //CHOICE C (#2)
        + > Do you have any unique tricks for processing graphics? # speaker: Player
            Absolutely! I'm actually really well known for reusing graphics to be more efficient, I'm super proud of it. # speaker: Suzy
            One piece of pixel data - let's say a robot - can be reused in multiple SCBs. I can draw it in different spots, scales, or colours without duplicating the actual image data. Saves memory and still looks great, Win-win! # speaker: Suzy
            -> Questions
        
        //CHOICE D (#3)
        + > Any chance I could see you in action? # speaker: Player
            I can do you one better! I'll let you try your hand at 'Collision Control'! # speaker: Suzy
            -> CollisionControlInstructions
        
    = CollisionControlInstructions
        Just press the Spacebar whenever you see two objects collide on-screen.
        Sounds simple but its quite tough! You sure you're ready? 
            + > Sure am! # speaker: Player
                I love your confidence, Let's get started! # speaker: Suzy
                ~ StartCollisionControl()
                -> DONE
            + > Not yet... # speaker: Player
                Oh, thats okay. Come back soon! # speaker: Suzy
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
                + > Yes. # speaker: Player
                    Good luck! # speaker: Suzy
                    ~ StartCollisionControl()
                    -> DONE
                + > No. # speaker: Player
                    Feel free to come back and try again whenever you'd like. # speaker: Suzy
                   ~ ExitDialogue()
                    -> DONE
        }
        
        

=== mikeyDialogue ===
    = FirstIntroduction
        Hi there, I am Mikey, very pleased to meet you. # speaker: Mikey
        Unlike my partner Suzy you may not see what I do, but you'd notice pretty fast if I stopped doing it. # speaker: Mikey
        I manage the game's logic, handle inputs, sound, as well as keeping Suzy on schedule. I run at 4 MHz - not flashy, but reliable. Suzy jokes that I'm slow, and she's not wrong. But I get a lot done with each cycle. # speaker: Mikey
        -> Questions
        
    = ReturningIntroduction
        Oh, you're back. Wanna give me a hand? # speaker : Mikey
            + > Could I ask you a question? # speaker : Player
                Yeah. # speaker: Mikey
                -> Questions
            + > Yeah, what do you need me to do? #speaker : Player
              We're doing 'Byte Runner', do you know it? # speaker : Mikey
                ++ > No, could you remind me? # speaker : Player
                    -> ByteRunnerInstructions
                ++ > I do! I've got this. # speaker: Player
                    ~ StartByteRunner()
                    -> DONE
    
    = Questions
        //CHOICE A (#0)
        + > How does an 8-bit CPU like you work? # speaker = Player
            I'm based on the 6502 family of processors. Classic architecture. I handle 8 bits of data at a time- meaning I process numbers from 0 to 255 in a single operation. # speaker: Mikey
            Larger numbers? More complex math? I break them down and handle them step by step. It's slower, sure, but it works. No floating point, no shortcuts. Just pure byte-by-byte logic. # speaker: Mikey
            I operate within a 16-bit address space, which gives me 64 kilobytes of memory. That's the total memory I can access at once - for code, variables, assets, everything. # speaker: Mikey
            It sounds small by today's standards, but with proper structure and clever loading, it's enough. And we were the pioneers of our time. Suzy, for instance, knows how to reuse graphics. I myself also admire that efficiency. # speaker: Mikey
            -> Questions
        
        //CHOICE B (#1)
        + > What kind of sound can you make? # speaker = Player
            I control a 4-channel sound system. Each channel plays its own waveform - like square waves, sawtooth, or noise. # speaker: Mikey
            You set the pitch, volume, and length, and I handle the playback. I can even mix in digital samples, though that's more demanding. I'm no synthesizer, but I do a good job delivering clean, retro audio. # speaker: Mikey
            -> Questions
            
        //CHOICE C (#2)
        + > What else can you do? # speaker = Player
            I work closely with Suzy, handling video timing - starting the screen refresh cycle, managing blanking intervals, and making sure Suzy doesn't draw mid-frame. # speaker: Mikey
            If you've ever seen screen-tearing in a game, that's what happens when graphics and timing go out of sync. I make sure that never happens here. # speaker: Mikey
            Although I handle a lot of tasks, handling anything more than one task at a time isn't my thing especially not how modern systems do. Don't get me wrong though I do move fast, so it feels smooth. # speaker: Mikey
            I run a main loop, one task at a time: check input, update logic, refresh screen, repeat. # speaker: Mikey
            -> Questions
            
        //CHOICE D (#3)
        + > Anything I can help you with? # speaker = Player
            Sure, it would be nice to have another helping hand around here. Would you like to try and allocate some bytes for me, experience for yourself how an 8-bit processor works? Join me in 'Byte Runner'! # speaker: Mikey
            -> ByteRunnerInstructions
            
    = ByteRunnerInstructions
        Don't worry it's simple, I'll send you address signals down the bus - you read 'em, and drag my byte workers into the right memory slots before Suzy hogs the line again! # speaker: Mikey
            + > I'm ready! # speaker: Player
                That's what I like to hear! # speaker: Mikey
                ~ StartByteRunner()
                -> DONE
            + > I need more time... # speaker: Player
                No stress, come back whenever you're ready. # speaker : Mikey
                ~ ExitDialogue()
                - > DONE
            
    -> DONE
    
