// PieChart.tsx
import React from 'react';
import { Pie } from 'react-chartjs-2';
import {
    Chart as ChartJS,
    ArcElement,
    CategoryScale,
    Title,
    Tooltip,
    Legend,
} from 'chart.js';

// Register the necessary components in Chart.js
ChartJS.register(ArcElement, CategoryScale, Title, Tooltip, Legend);

function PieChart({ labels, values }: { labels: string[], values: number[] }) {
    // Sample data for the pie chart
    const data = {
        labels: labels,
        datasets: [
            {
                data: values,
                backgroundColor: ['#F5E39E', '#B9C9F5', '#9DF58E', '#CFD0D6'],
                hoverOffset: 4,
            },
        ],
    };

    // Pie chart options (optional, you can adjust as needed)
    const options = {
        responsive: true,
        plugins: {
            legend: {
                position: 'top' as const,
            },
            tooltip: {
                enabled: true,
            },
        },
    };

    return (
        <div className="max-w-md mx-auto p-4 rounded-lg shadow-lg bg-white">
            <h2 className="text-center text-2xl font-bold text-gray-800 mb-4">Simple Pie Chart</h2>
            <div className="flex justify-center items-center">
                <Pie data={data} options={options} />
            </div>
        </div>
    );
};

export default PieChart;
