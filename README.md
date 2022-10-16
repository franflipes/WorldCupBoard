# WorldCupBoard

## GENERAL
There is an empty project called "WorldCupBoard". My first intention was to create a command line interactive projects where to run an application to intercat with the library. Later on, reading carefully the instructions I understood that the library and the testing project were enough for the exercise and to **keep it simple**.

I didnt think appropiate to use try-catch-finally, eventhough it is always good practice, the size of the solution is ridicuolous and I tried to cover everything with Testing.

If you look at the word "REQUIREMENT" through the code, you will find where the 4 requirements are implemented, both in library project and in Testing project.

Public methods in managers have XML documentation.

## STRUCTURE
Pretty straighforward implementation. A library that holds 2 classes(**Game and Team**) for the basic information. 2 more classes called managers(**GameManager and TeamManager**) that implement the logic of the application and the classes in charge of interact with TEAMs and GAMEs

### Library Project
-**TEAM**: just hold the name. <br />
-**GAME**: constructor is private. The idea behind is that we encapsulate the constructor in a static call Game.Start().<br /> 
&nbsp; We could have done this in the GamesManager which basically manage the Games and interact with them <br />
&nbsp; We also update scores and get Scores in a tuple<int,int>. <br />
&nbsp; Finish() method is not used, we thought a way to have an endTime, having this we could display in the dashboard the ones that are not <br />
&nbsp; finished and in that case we don´t need to remove in the GamesManager collection. Also, having this variable in use in the class, could make <br />
&nbsp;  us to ask if a Game is in progress to the class itself. At the end I stuck to the simplest solution.<br />
-**GAMES MANAGER**: Basically this class is the **Scoreboard**. Hold the logic to interact with Games. It also has a collection of GAMEs.<br /> 
 &nbsp; We have some auxiliar methods to check if a game or a team are already in playing or to retrieve an already game in progress. <br />
 &nbsp; Returning the GamesSummary ordered as specified. <br />
-**TEAMS MANAGER**:  Hold the logic to interact with Teams. It also has a collection of TEAMs. It manages the creation and removal of TEAMs.<br />
&nbsp; Again with some auxiliar methods that helps to control the collection of TEAMs


### Testing Project
-I have tried to cover as much testing as possible. I Didn´t do any testing for Team class because it is not worth. <br />
&nbsp;**GameUnitTest** class have 3 Nominal Tests. <br />
&nbsp;So, most of the testing is again for the managers where is implemented most of the logic for the scoreboard. <br />
-I tried whenever I could to test not only nominal cases but also not nominal use cases.<br />
-Make some improvements using initialize methods and clean-ups


