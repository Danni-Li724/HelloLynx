VAR collisionControl_Victory = false

EXTERNAL ExitDialogue()
EXTERNAL StartCollisionControl()
EXTERNAL StartByteRunner()
EXTERNAL AwardCollisionControlBadge()
EXTERNAL AwardByteRunnerBadge()

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
        //CHOICE A
        + > How do you draw sprites? # speaker: Player
            Okay, picture this: You want a spaceship zooming across the screen. You don't just throw me an image and say "go". I need two things: # speaker: Suzy
            1. Pixel data - the raw visual. # speaker: Suzy
            2. A Sprite Control Block - basically a set of instructions telling me how to handle it. # speaker: Suzy
            Give me those, and I'll blit that thing onto the screen buffer in no time. # speaker: Suzy
            -> Questions
        
        //CHOICE B
        + > How does collision detection work? # speaker: Player
            Ah, my secret sauce! I have a technique that no other consoles at my time have figured out. # speaker: Suzy
            While I'm drawing, I also keep an eye out for overlaps. Each sprite gets a collision number - sort of like a nametag, numbered 0 to 15. # speaker: Suzy
            If two sprites overlap, I record the highest number into something called the collision depository. You (or your game code) can check it afterward to see what hit what. And yes - this is pixel-perfect. None of that rectangle-guessing nonsense other consoles are used to. # speaker: Suzy
            -> Questions
        
        //CHOICE C
        + > Do you have any unique tricks for processing graphics? # speaker: Player
            Absolutely! I'm actually really well known for reusing graphics to be more efficient, I'm super proud of it. # speaker: Suzy
            One piece of pixel data - let's say a robot - can be reused in multiple SCBs. I can draw it in different spots, scales, or colours without duplicating the actual image data. Saves memory and still looks great, Win-win! # speaker: Suzy
            -> Questions
        
        //CHOICE D
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
            + > I need more time... # speaker: Player
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