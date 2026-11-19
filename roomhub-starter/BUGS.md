# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
|R6-a rezervation of more then 4 hours is not declined because such a condition is not verified| in the BookingService.cs, should be after the validation at line 17| I discovered it by reading the business rules and compared it to the starter file | _(fill in)_ |
|R8-Cancelling an already cancelled reservation passes|in BookingService.cs lines 50-60 |AI|added the verification in lines 66-67 |
|R4-TimeProvider isn t used , bookings can start in the past like this|in BookingService.cs lines 8-25|reading the code (there is no comparison between startUtc and now)|added TimeProvider + lines for the verification at lines 36-37|

