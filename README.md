# Vinh's JunkyardAPI

Hello! And welcome to my basic web API built with ASP.NET Core

This API will be incrementally built on to complete the Phase 2 learning objectives.
It is modelled to recreate a Junkyard's inventory system where Cars and their Parts need to be tracked.

In its current form, it uses Entity Framework hooked up to an In-memory Database to manage the entries. There are future plans to implement 
a PostgreSQL database when completing CI/CD learning objectives.

## Requirements

- Simple CRUD API to store cars and parts
- Cars can have many parts associated to them

## Design Choices

Throughout building this API, there were plenty of forks in the road where multiple approaches could be implemented and the pros and cons of each
were to be considered before continuing. In this section, I will outline my thoughts and justifications for each design choice made

### Nested Objects

This was the first and probably big one. How should controllers be managed when Parts are a sub-object of Cars? 
Should Parts endpoints be managed by the CarsController?
Should Parts have their own controller? How would the routing work?

One could make the argument that the Parts endpoints should live within the CarsController as you would not be able to access the Parts without first
going through a Car. After all, a Part would not exist without a Car.

Initially I did implement my Controller using this approach, however I soon found the CarsController to be quite bloated and difficult to manage. 
It became unclear where the Cars endpoints stopped and Parts endpoints began. Once I seperated the Controllers, it became much more manageable 
and maintainable. It was then clear which entity the endpoint would return, as it was specified in the Controller name.

This is not to say I would never bundle my parent/child entities into one controller. I could see a use case where there may be many more than 
just two entities and having a large number of Controllers could be difficult to quickly see which ones are related.

### HTTP POST/PUT

Comparing to other APIs, I've taken the approach of implementing POST/PUT differently. For a PUT request, while most have chosen to return NotFound
when the given ID does not exist, it would make sense to me to create a new entry with that given ID (as the documentation suggests 
[here](https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/PUT)).

In doing this, I wanted to reuse the logic I had for the POST method as it should be the same, except added in an optional parameter for ID
to create the new entity. This reduces the amount repeated code in my controller. It also allowed the user to create an entry with their own specified
ID with PUT and POST, should they want to. 

The main limitation with this approach is, there is nothing stopping the user from creating an entry with an absurdly high number as the ID. EF core
will then auto increment from the highest number to avoid issues when it reaches the occupied number.

### The Service Layer

Ah the service layer, the place where all the "business logic" for the entities live. Unfortunately my API does not contain much business logic, if any.
So why then did I decide to include it I hear you ask. As mentioned above, I allowed the users to create an entry with a PUT request if the entry did
not yet exist. When performing a PUT request on a Part for example, we need to check if the part exists for the given car and if not, if it exists
in the wider database and only then should we create it.

Without the service layer, this validation responsibilty would fall on either the Controller or the Repository. This validation logic does not belong in
the Controller so the only other option would be the Repository. It did not feel right to have the PartsRepository query the database regarding 
information about the Car, when we have a perfectly good CarsRepository which handles these queries. It also did not feel right to inject the 
CarsRepository into the PartsRepository in order to have access to those capabilities.

Enter, the service layer. It was here where we could inject both repositories to query the database and handle the information into neat validation logic
the controller could process. Works well for the Parts entity, not so well for the Cars where the service layer looks a lot like straight method calls
to the single CarsRepository. It was probably not necessary to include a CarsService, but I did so out of symmetry to the PartsService.
I could also make the argument that if future business logic were to arise, my code is more extensible due to its presence, despite what YAGNI says.

### Versioning

So many ways to implement versioning, how can you choose which one to use. I decided to accept the API version through the request headers. This was
mentioned by my mentor as MYOB's versioning method through their public API so why not implement it on my own. 

Initially, I had opted to implement versioning through the URI route as it seemed intuitive. However, I noticed a nice advantage by versioning through
the headers had over through the route. I could set a default version if the version was unspecified and this allowed my API to be backwards 
compatible if the user did not know about the existence of the new version, as in the user would not have to change a thing in order to continue using
the API. If I had versioned through the route, even the original v1 API would require the version to be specified in the route; potentially breaking
their API use.

## Running the project

How could this be considered a README if I didn't include instructions on how to run this project. At the moment, this project can only be run locally.

### Installing .NET6

You will need to have the .NET6 SDK installed on your computer.

Install the .NET SDK using homebrew on the command line: `brew install --cask dotnet-sdk`

Alternatively, you can download the .NET SDK [here](https://dotnet.microsoft.com/en-us/)

### Set up 

First clone this repository to your local machine:
```
git clone git@github.com/myob-fma/Vinh-JunkyardWebApp.git
```

To start up the API locally, use the command `dotnet run` from the CLI when inside the `JunkyardWebApp.API/` directory. A Swagger page should open
up in your broswer window.
