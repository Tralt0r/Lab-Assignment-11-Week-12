Carnival Game



Xavier Rosario



This is our little carnival shooting game. You move a shooter left and right along the bottom and shoot at enemies for points.



How I used the patterns



Object Pool

Instead of making a new bullet every time you shoot, we make 8 bullets at the start and hide them. When you shoot, the shooter grabs one of the hidden ones and fires it. When the bullet hits something or flies off the top of the screen, it hides itself again and goes back in the pile so it can be used later. We never create or destroy bullets while playing. If all 8 are out at once, you just can't shoot until one comes back. There's a counter on the screen so you can watch it happen.



Builder

Targets have a bunch of settings, so I used a builder to put them together one piece at a time. You chain the settings and then call Build() to get the target. I also made a TargetDirector script that has the premade ones:

Enemy One: slow and big, 10 points

Enemy Two: medium, 25 points

Enemy Three: fast and small, 50 points



The spawner just asks the director for a random one every 0.6 seconds to 1.4 seconds.



Observer

When a bullet hits a target, it doesn't update the score itself. It just announces "hey a target got hit" through GameEvents.Hits.Notify(). Anything that signed up to listen gets the message. Right now there is one listener, ScoreManager adds the points. Scripts sign up in `OnEnable` and sign off in `OnDisable`. The cool part is the bullet has no idea the score even exists, so we could add more listeners later without touching the bullet.

