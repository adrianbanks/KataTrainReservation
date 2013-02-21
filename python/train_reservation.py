
def reserve_seats(train, reservation_request):
    # TODO: write this code!
    pass

class Train(object):
    def __init__(self, carriages=None, maximum_reservation_percentage=70):
        self.carriages = carriages or []
        self.maximum_reservation_percentage = maximum_reservation_percentage
        
class ReservationRequest(object):
    def __init__(self, seatCount):
        self.seatCount = seatCount

class Carriage(object):
    def __init__(self, capacity):
        self.capacity = capacity
        self.seats = [Seat(self) for i in range(capacity)]
        
class Seat(object):
    def __init__(self, carriage):
        self.carriage = carriage
        
class ReservationException(Exception):
    pass