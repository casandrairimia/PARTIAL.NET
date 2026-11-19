# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

**R1.** A room has a non-empty name and a strictly positive capacity. This validation should have been made in the domain class because multiple services can go through the same validation of the room

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

We don t want the program to stop, we want the user to pe informed why a certain action can't be performed instead of stopping the program(Error)
## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Interfetele repositoryurilor (IBookingRepository , IRoomRepository), DbContext, fisirele din persistence
cost : mai multe interfet
benefit : accesul la date e separat. 

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings
→ CreateBookingEndpoint.MapCreateBooking
→ CreateBookingRequest
→ CreateBookingCommand
→ CreateBookingHandler.Handle
→ IRoomRepository.Get
→ EfRoomRepository.Get
→ RoomHubDbContext.Rooms
→ SQLite: reads the room
→ IBookingRepository.ListForRoom
→ EfBookingRepository.ListForRoom
→ RoomHubDbContext.Bookings
→ SQLite: 
→ Booking.Create
→ IBookingRepository.Add
→ EfBookingRepository.Add
→ IUnitOfWork.SaveChanges
→ EfUnitOfWork.SaveChanges
→ RoomHubDbContext.SaveChanges
→ SQLite

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

I rejected the addition of code in the service file, this will no longer be necessary

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?
 adaugarea testelor de integrare
