# Visma .NET Internship Task
A coding task gotten while applying for an internship position at Visma

## Opening the solution
The solution is in /VismaMeetingsTask/VismaMeetingsTask.sln
With this you can open the project and try out the program.
## Task
Create a console application to manage Visma’s internal meetings using .NET6.

### Requirements
```
● Command to create a new meeting. All the meeting data should be stored in a JSON
  file. Application should retain data between restarts. Meeting model should contain
  the following properties:
    ○ Name
    ○ ResponsiblePerson
    ○ Description
    ○ Category (Fixed values - CodeMonkey / Hub / Short / TeamBuilding)
    ○ Type (Fixed values - Live / InPerson)
    ○ StartDate
    ○ EndDate
● Command to delete a meeting. Only the person responsible can delete the meeting
● Command to add a person to the meeting.
    ○ Command should specify who is being added and at what time.
    ○ If a person is already in a meeting which intersects with the one being added, a warning message should be given.
    ○ Prevent the same person from being added twice.
● Command to remove a person from the meeting.
    ○ If a person is responsible for the meeting, he can not be removed.
● Command to list all the meetings.
    ○ Filter by description (if the description is “Jono .NET meetas”, searching for
    .NET should return this entry)
    ○ Filter by responsible person
    ○ Filter by category
    ○ Filter by type
    ○ Filter by dates (e.g meetings that will happen starting from 2022-01-01 /
    meetings that will happen between 2022-01-01 and 2022-02-01)
    ○ Filter by the number of attendees (e.g show meetings that have over 10
people attending)
```
