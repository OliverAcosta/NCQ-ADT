

create TABLE Notes(
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  UserTaskId INTEGER not null,
  Note varchar(600) not null, 
  Active bit not null
);

create TABLE Priorities(
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name varchar(200) not null,
  Description varchar(600) null,
  Active not null
);

create TABLE Status(
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name int not null,
  Description varchar(600) null,
  Active bit not null
);

CREATE TABLE User(
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Username varchar(200) not null,
  Name varchar(200) not null,
  Email varchar(200) null,
  Password varchar(800),
  Active bit not null
);

CREATE TABLE UserTask(
  Id INTEGER PRIMARY KEY AUTOINCREMENT,
  Name varchar(200) not null,
  Description varchar(600) null,
  Created Datetime not null,
  DueDate datetime not null,
  UserId INTEGER not null,
  StatusId INTEGER not null,
  PriorityId INTEGER not null
)


  