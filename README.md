# Projet C# - Pokemon Game

I'm Axel Michon, here is my project for this work: 

It uses EntityFramework (SqlServer, Tools, Design, Core) and CommunityToolKit.MVVM

When you'll start the project, the first page you'll get will be the Database Setter.
This is here that you put your server name for the database connection. Unfortunately,
I couldn't find a way to prevent crash if an wrong name is entered, so be careful to enter
a valid name :)

After entering your database server, you'll get to the login screen, if you don't have any account yet, no worries, you just have to click on the "No Account?" link at the bottom.

The Register and Login pages are very similar. The main difference is that the register page adds you to the database and the login page just verify if the informations you've just entered are correct.

After login in or registered, you'll be at the main menu! Here you have 3 buttons: Start, Pokedex and Settings.

Settings: There is only a way to reset the database if needed.

Pokedex: There you'll see every pokemon currently available, check their stats and spells as much as spells details

Start: The main dish!


Additional infos:

When you reset the database, all id will restart from 0 and pre-made values will be added automatically.

In pokedex, you can click on any pokemon to see more details about it, and on the pokemon details you can click on any spell to see full details on it as well! Don't forget the little button to the left of the pokemon picture, it's used to select this pokemon to use it in battle!

In combat, you can choose any of the ability available for you pokemon. After choosing one, the game will choose which pokemon will start attacking (2/3 for the player to start) and then will deals damage accordingly and will update the health bar. If your health reach 0 before your opponent, you loose and will then be send back to the main menu. If the opponent's health reach 0 before you do, then you win! GG! Now 2 buttons will appear in the center, one leads you to the main menu and the other one will make another pokemon appears! When winning a fight you'll get a random buff on your health or damage and can even get a potion (50/50) to regen half of your max hp! But... Next opponent do gets buff as well based on the number of waves, their buffs are less good than the players ones but this doesn't make the game easier...


PS: The combat system can't be simplier than this, no type, no evolutions or whatsoever, it isn't good in my opinion but hope it's enough. Also the pages aren't responsible so twiking the pages will make them really bad looking (still playable tho).
