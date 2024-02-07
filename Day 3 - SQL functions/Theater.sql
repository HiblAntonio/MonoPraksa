CREATE TABLE "Customer"(
	"Id" SERIAL PRIMARY KEY,
	"Firstname" VARCHAR(50) NOT NULL,
	"Lastname" VARCHAR(50) NOT NULL,
	"Dob" TIMESTAMP
);

CREATE TABLE "Ticket"(
	"Id" SERIAL PRIMARY KEY,
	"CustomerId" INT NOT NULL,
	CONSTRAIN "FK_Ticket_Customer_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customer"("Id"),
	"Price" FLOAT NOT NULL,
	"SeatNumber" INT UNIQUE NOT NULL
);

CREATE TABLE "Theater"(
	"Id" SERIAL PRIMARY KEY,
	"Address" VARCHAR(100) NOT NULL,
	"Capacity" INT,
	"Size" FLOAT
);

CREATE TABLE "Actor"(
	"Id" SERIAL PRIMARY KEY,
	"FirstName" VARCHAR(100) NOT NULL,
	"LastName" VARCHAR(100) NOT NULL,
	"Dob" TIMESTAMP,
	"Salary" FLOAT
);

CREATE TABLE "Genre"(
	"Id" SERIAL INT PRIMARY KEY,
	"Type" VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE "Play"(
	"Id" SERIAL PRIMARY KEY,
	"GenreId" INT NOT NULL,
	CONSTRAIN "FK_Play_Genre_GenreId" FOREIGN KEY ("GenreId") REFERENCES "Genre"("Id"),
	"Title" VARCHAR(100) NOT NULL
);

CREATE TABLE "Performance"(
	"Id" SERIAL PRIMARY KEY,
	"ActorId" INT NOT NULL,
	"TheaterId" INT NOT NULL,
	"PlayId" INT NOT NULL,
	CONSTRAIN ormance_Actor_ActorId" FOREIGN KEY ("ActorId") REFERENCES "Actor"("Id"),
	CONSTRAIN "FK_Performance_Theater_TheaterId" FOREIGN KEY ("TheaterId") REFERENCES "Theater"("Id"),
	CONSTRAIN "FK_Performance_Play_PlayId" FOREIGN KEY ("PlayId") REFERENCES "Play"("Id"),
	"Time" TIMESTAMP NOT NULL,
	"Date" DATE NOT NULL
);