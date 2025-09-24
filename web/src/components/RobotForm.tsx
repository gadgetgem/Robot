import { useState, type ChangeEvent } from 'react';

import axios from 'axios';

type SimulationResult = {
  finalLocation: string;
};

export default function RobotForm() {
  const [gridX, setGridX] = useState<number | null>(null);
  const [gridY, setGridY] = useState<number | null>(null);
  const [robots, setRobots] = useState<
    { x: number; y: number; orientation: string; commands: string }[]
  >([]);
  const [results, setResults] = useState<SimulationResult[]>([]);

  const addRobot = () => {
    setRobots([...robots, { x: 0, y: 0, orientation: "N", commands: "" }]);
  };

  const updateRobot = (index: number, field: string, value: string | number) => {
    const updated = [...robots];
    (updated[index] as any)[field] = value;
    setRobots(updated);
  };

  const handleGridChange = (setter: (v: number | null) => void) => (e: ChangeEvent<HTMLInputElement>) => {
    const raw = e.target.value;
    if (raw === '') {
      setter(null); 
      return;
    }
    const num = Number(raw);
    if (!Number.isNaN(num)) {
      setter(num);
    }

  };

  const isValidGrid = (v: number | null) => typeof v === 'number' && v >= 0;
  const isValid = isValidGrid(gridX) && isValidGrid(gridY) && robots.length > 0;

  const moveRobot = async () => {
    if (!isValid) return; 
    const payload = {
      gridSize: { maxX: gridX, maxY: gridY },
      robots: robots.map(r => ({
        robotStart: { x: r.x, y: r.y, orientation: r.orientation     },
        commands: r.commands
      }))
    };
    const res = await axios.post<SimulationResult[]>("https://localhost:7169/api/robot/commands", payload);
    setResults(res.data);   
  };

  return (
    <div className="p-6 max-w-3xl mx-auto">  
  <div className="mb-4 flex flex-wrap items-center gap-4 text-sm">  
    <label className="font-medium mb-2">Grid Size:</label>  
      <input
        type="number"
        value={gridX ?? ''}
        onChange={handleGridChange(setGridX)}
        placeholder="Max X"
        className="border rounded px-2 py-1 w-24 focus:outline-none focus:ring-1 focus:ring-blue-500"
      />
      <input
        type="number"
        value={gridY ?? ''}
        onChange={handleGridChange(setGridY)}
        placeholder="Max Y"
        className="border rounded px-2 py-1 w-24 focus:outline-none focus:ring-1 focus:ring-blue-500"
      />
    </div>

      <button onClick={addRobot} className="bg-blue-500 text-white px-3 py-1 rounded mb-4 mt-4">+ Add Robot</button>

      {robots.map((r, i) => (
        <div key={i} >
          <h2 className="font-semibold">Robot {i+1}</h2>
          <div className="">
            <input type="number" value={r.x} onChange={e => updateRobot(i, "x", +e.target.value)} width={80}/>
            <input type="number" value={r.y} onChange={e => updateRobot(i, "y", +e.target.value)} className="border rounded px-1 py-0.5 w-16 focus:outline-none focus:ring-1 focus:ring-blue-500"/>
            <select value={r.orientation} onChange={e => updateRobot(i, "orientation", e.target.value)} className="border rounded px-1 py-0.5 w-16 bg-white focus:outline-none focus:ring-1 focus:ring-blue-500">
              <option value="N">N</option>
              <option value="E">E</option>
              <option value="S">S</option>
              <option value="W">W</option>
            </select>
            <input type="text" value={r.commands} onChange={e => updateRobot(i, "commands", e.target.value)} placeholder="Commands" className="border rounded px-2 py-1 w-52 md:w-64 focus:outline-none focus:ring-1 focus:ring-blue-500"/>
          </div>
        </div>
      ))}

  <button onClick={moveRobot} disabled={!isValid}>Move Robots</button>

      <div className="mt-6">
        <h2 className="text-xl font-bold">Results</h2>
        <ul className="list-disc ml-6">
          {results.map((r, i) => (
            <li key={i}>{r.finalLocation}</li>
          ))}
        </ul>
      </div>
    </div>
  );
}