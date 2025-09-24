# Robot

Mars is a planet, this one happens to be a Square

With a grid of max size 50 x 50 Place Robots onto the grid with a start position of xy and an orientation of N, S, E, W Move the Robot with a list of commands: L -> Turn Left, R -> Turn Right or F -> Move Forward

If the Robot falls of the grid the last location known will be saved with a scent and returned as output with LOST If there is a scent then the Robot will ignore the command and continue with the rest of the commands given

The Robot will report it's final location and output as part of array of all other Robots on the same Grid

This app uses an API with the endpoint https://localhost:7169/api/robot/commands

This can be called from the endpoint explorer in visual studio or directly as POST request with the following json format:

{"gridSize": { "MaxY": 5, "height": 3 },
"robots": [{ "robotStart": {
                "x": 1, 
                "y": 1, 
                "orientation": "E"},
            "commands":"RFRFRFRF" }}
]]

There is also a very simple web version with a form to submit requests.


# Presumptions

If a robot is recorded as lost then it will be recorded in a scent
If a subsequent robot is recorded as lost then it will ignore the last command and continue

# Further Considerations
What happens if two robots if two robots try to occupy the same square?
Add logging
Consider bottlenecks 
Api retry with Polly
