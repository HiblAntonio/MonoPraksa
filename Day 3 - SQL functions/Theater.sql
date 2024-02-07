CREATE TABLE "Customer"(
	"Id" UUID PRIMARY KEY,
	"Firstname" VARCHAR(50) NOT NULL,
	"Lastname" VARCHAR(50) NOT NULL,
	"Dob" TIMESTAMP
);

CREATE TABLE "Ticket"(
	"Id" UUID PRIMARY KEY,
	"CustomerId" UUID NOT NULL,
	CONSTRAINT "FK_Ticket_Customer_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customer"("Id"),
	"Price" FLOAT NOT NULL,
	"SeatNumber" INT UNIQUE NOT NULL
);

CREATE TABLE "Theater"(
	"Id" UUID PRIMARY KEY,
	"Address" VARCHAR(100) NOT NULL,
	"Capacity" INT,
	"Size" FLOAT
);

CREATE TABLE "Actor"(
	"Id" UUID PRIMARY KEY,
	"FirstName" VARCHAR(100) NOT NULL,
	"LastName" VARCHAR(100) NOT NULL,
	"Dob" TIMESTAMP,
	"Salary" FLOAT
);

CREATE TABLE "Genre"(
	"Id" UUID PRIMARY KEY,
	"Type" VARCHAR(50) UNIQUE NOT NULL
);

CREATE TABLE "Play"(
	"Id" UUID PRIMARY KEY,
	"GenreId" UUID NOT NULL,
	CONSTRAINT "FK_Play_Genre_GenreId" FOREIGN KEY ("GenreId") REFERENCES "Genre"("Id"),
	"Title" VARCHAR(100) NOT NULL
);

CREATE TABLE "Performance"(
	"Id" UUID PRIMARY KEY,
	"ActorId" UUID NOT NULL,
	"TheaterId" UUID NOT NULL,
	"PlayId" UUID NOT NULL,
	CONSTRAINT "Performance_Actor_ActorId" FOREIGN KEY ("ActorId") REFERENCES "Actor"("Id"),
	CONSTRAINT "FK_Performance_Theater_TheaterId" FOREIGN KEY ("TheaterId") REFERENCES "Theater"("Id"),
	CONSTRAINT "FK_Performance_Play_PlayId" FOREIGN KEY ("PlayId") REFERENCES "Play"("Id"),
	"Time" TIMESTAMP NOT NULL,
	"Date" DATE NOT NULL
);