import React, { useState, useEffect } from 'react';

// Enum types to match your C# model
enum Priority {
    Low = 0,
    Medium = 1,
    High = 2
}

enum Status {
    Pending,
    InProgress,
    Completed,
    Archived
}

// Interface to match TaskRecord
interface TaskRecord {
    taskId: number;
    title: string;
    description?: string;
    priority: Priority;
    dueDate: string;
    status: Status;
}

// Interface for task form (works for both new and update)
interface TaskForm {
    title: string;
    description: string;
    priority: Priority;
    dueDate: string;
    status: Status;
}

function TaskGrid({ jwtToken }: { jwtToken: string }) {
    const [tasks, setTasks] = useState<TaskRecord[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    // State for modal
    const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
    const [isUpdateMode, setIsUpdateMode] = useState<boolean>(false);
    const [isAlertOpen, setIsAlertOpen] = useState<boolean>(false);
    const [isHighPriority, setIsHighPriority] = useState<boolean>(false);
    const [currentTaskId, setCurrentTaskId] = useState<number | null>(null);
    const [taskForm, setTaskForm] = useState<TaskForm>({
        title: '',
        description: '',
        priority: Priority.Medium,
        dueDate: new Date().toISOString().split('T')[0],
        status: Status.Pending
    });

    // Fetch tasks from API
    const fetchTasks = async () => {
        try {
            const response = await fetch('tasks', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${jwtToken}`,
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const data = await response.json();
            setTasks(data);
            setLoading(false);
        } catch (err) {
            console.error('Fetch error:', err);
            setError(err instanceof Error ? err.message : 'An unknown error occurred');
            setLoading(false);
        }
    };

    useEffect(() => {
        if (jwtToken) {
            fetchTasks();
        }
        // If here before logout
    }, [jwtToken]);

    // Reset form to default values
    const resetForm = () => {
        setTaskForm({
            title: '',
            description: '',
            priority: Priority.Medium,
            dueDate: new Date().toISOString().split('T')[0],
            status: Status.Pending
        });
        setCurrentTaskId(null);
        setIsUpdateMode(false);
        setIsModalOpen(false);
        setIsAlertOpen(false);
        setIsHighPriority(false);
    };

    // Open modal for creating a new task
    const openCreateModal = () => {
        resetForm();
        setIsUpdateMode(false);
        setIsModalOpen(true);
        setIsAlertOpen(false);
    };

    // Open modal for updating an existing task
    const openUpdateModal = (task: TaskRecord) => {
        setTaskForm({
            title: task.title,
            description: task.description || '',
            priority: task.priority,
            dueDate: new Date(task.dueDate).toISOString().split('T')[0],
            status: task.status
        });
        setCurrentTaskId(task.taskId);
        setIsUpdateMode(true);
        setIsModalOpen(true);
        setIsAlertOpen(false);
    };

    const openDeleteModal = (task: TaskRecord) => {
        setTaskForm({
            title: task.title,
            description: task.description || '',
            priority: task.priority,
            dueDate: new Date(task.dueDate).toISOString().split('T')[0],
            status: task.status
        });
        setCurrentTaskId(task.taskId);
        setIsUpdateMode(false);
        setIsModalOpen(false);
        setIsAlertOpen(true);
    };

    // Handle creating a new task
    const handleCreateTask = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            const response = await fetch('tasks', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${jwtToken}`,
                },
                body: JSON.stringify(taskForm)
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Refresh tasks list
            await fetchTasks();

            resetForm();
        } catch (err) {
            console.error('Create task error:', err);

            if (err.message.includes('status: 403')) {
                alert('You do not have permission to do this action');
            }
            else {
                alert(`Error : ${err.message}`);
            }

            resetForm();
        }
    };

    // Handle updating an existing task
    const handleUpdateTask = async (e: React.FormEvent) => {
        e.preventDefault();

        if (currentTaskId === null) {
            console.error('No task ID for update');
            return;
        }

        try {
            const response = await fetch(`tasks/${currentTaskId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${jwtToken}`,
                },
                body: JSON.stringify({
                    taskId: currentTaskId,
                    ...taskForm
                })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Refresh tasks list
            await fetchTasks();

            resetForm();
        } catch (err) {
            console.error('Update task error:', err);

            if (err.message.includes('status: 403')) {
                alert('You do not have permission to do this action');
            }
            else {
                alert(`Error : ${err.message}`);
            }

            resetForm();
        }
    };

    const handleSubmit = (e: React.FormEvent) => {
        if (isUpdateMode) {
            handleUpdateTask(e);
        } else {
            handleCreateTask(e);
        }
    };

    const handleDeleteTask = async () => {
        try {
            const response = await fetch(`tasks/${currentTaskId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${jwtToken}`,
                }
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            // Refresh tasks list
            await fetchTasks();

            resetForm();
        } catch (err) {
            console.error('Delete task error:', err);

            if (err.message.includes('status: 403')) {
                alert('You do not have permission to do this action');
            }
            else {
                alert(`Error : ${err.message}`);
            }

            resetForm();
        }
    };

    // Helper function to get priority color
    const getPriorityColor = (priority: Priority) => {
        switch (priority) {
            case Priority.Low:
                return 'bg-green-100 text-green-800';
            case Priority.Medium:
                return 'bg-yellow-100 text-yellow-800';
            case Priority.High:
                return 'bg-red-100 text-red-800';
            default:
                return 'bg-gray-100 text-gray-800';
        }
    };

    // Helper function to get status color
    const getStatusColor = (status: Status) => {
        switch (status) {
            case Status.Pending:
                return 'bg-yellow-100 text-yellow-800';
            case Status.InProgress:
                return 'bg-blue-100 text-blue-800';
            case Status.Completed:
                return 'bg-green-100 text-green-800';
            case Status.Archived:
                return 'bg-gray-100 text-gray-800';
            default:
                return 'bg-gray-100 text-gray-800';
        }
    };

    if (loading) {
        return <div className="p-4">Loading tasks...</div>;
    }

    if (error) {
        return (
            <div className="p-4 text-red-500">
                Error: {error}
                <pre className="mt-2 bg-gray-100 p-2 rounded">{error}</pre>
            </div>
        );
    }

    return (
        <div className="w-full p-4">
            {/* Create Task Button */}
            <div className="mb-4">
                <button
                    onClick={openCreateModal}
                    className="bg-blue-500 hover:bg-blue-700 text-black font-bold py-2 px-4 rounded"
                >
                    Create New Task
                </button>
            </div>

            {/* Task Modal (Create or Update) */}
            {isModalOpen && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
                    <div className="bg-white p-6 rounded-lg w-96">
                        <h2 className="text-xl font-bold mb-4">
                            {isUpdateMode ? 'Update Task' : 'Create New Task'}
                        </h2>
                        <form onSubmit={handleSubmit}>
                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="title">
                                    Title
                                </label>
                                <input
                                    type="text"
                                    id="title"
                                    value={taskForm.title}
                                    onChange={(e) => setTaskForm({ ...taskForm, title: e.target.value })}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    required
                                />
                            </div>

                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="description">
                                    Description
                                </label>
                                <textarea
                                    id="description"
                                    value={taskForm.description}
                                    onChange={(e) => setTaskForm({ ...taskForm, description: e.target.value })}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                />
                            </div>

                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="priority">
                                    Priority
                                </label>
                                <select
                                    id="priority"
                                    value={taskForm.priority}
                                    onChange={(e) => {
                                        setTaskForm({ ...taskForm, priority: Number(e.target.value) as Priority });
                                        if (Number(e.target.value) == 2) {
                                            setIsHighPriority(true);
                                        }
                                        else {
                                            setIsHighPriority(false);
                                        }
                                    }}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                >
                                    <option value={Priority.Low}>Low</option>
                                    <option value={Priority.Medium}>Medium</option>
                                    <option value={Priority.High}>High</option>
                                </select>
                            </div>

                            {isHighPriority &&
                                <div class="p-4 mb-4 text-sm text-yellow-800 rounded-lg bg-yellow-50 dark:bg-gray-800 dark:text-yellow-300" role="alert">
                                    <span class="font-medium">Warning!</span> Are you sure you want to set the task to such a high priority?
                                </div>
                            }

                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="dueDate">
                                    Due Date
                                </label>
                                <input
                                    type="date"
                                    id="dueDate"
                                    value={taskForm.dueDate}
                                    onChange={(e) => setTaskForm({ ...taskForm, dueDate: e.target.value })}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                    required
                                />
                            </div>

                            <div className="mb-4">
                                <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="status">
                                    Status
                                </label>
                                <select
                                    id="status"
                                    value={taskForm.status}
                                    onChange={(e) => setTaskForm({ ...taskForm, status: Number(e.target.value) as Status })}
                                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                >
                                    <option value={Status.Pending}>Pending</option>
                                    <option value={Status.InProgress}>In Progress</option>
                                    <option value={Status.Completed}>Completed</option>
                                    <option value={Status.Archived}>Archived</option>
                                </select>
                            </div>

                            <div className="flex items-center justify-between">
                                <button
                                    type="submit"
                                    className="bg-blue-500 hover:bg-blue-700 text-black font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                >
                                    {isUpdateMode ? 'Update Task' : 'Create Task'}
                                </button>
                                <button
                                    type="button"
                                    onClick={() => {
                                        setIsModalOpen(false);
                                        resetForm();
                                    }}
                                    className="bg-gray-500 hover:bg-gray-700 text-black font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                                >
                                    Cancel
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}

            { /* Alert Delete */}
            {isAlertOpen && (
                <div className="fixed inset-0 flex justify-center items-center z-50">
                    <div className="bg-white p-6 rounded-lg w-96">
                        <h2 className="text-xl font-bold mb-4">
                            Are you sure you want to delete this task?
                        </h2>

                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="title">
                                Title
                            </label>
                            <p id="title" className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline">
                                {taskForm.title}
                            </p>
                        </div>

                        <div className="mb-4">
                            <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="title">
                                Description
                            </label>
                            <p id="title" className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline">
                                {taskForm.description}
                            </p>
                        </div>

                        <div className="flex items-center justify-between">
                            <button
                                type="button"
                                onClick={() => {
                                    handleDeleteTask()
                                }}
                                className="bg-blue-500 hover:bg-blue-700 text-black font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                            >
                                Yes
                            </button>
                            <button
                                type="button"
                                onClick={() => {
                                    setIsAlertOpen(false);
                                    resetForm();
                                }}
                                className="bg-gray-500 hover:bg-gray-700 text-black font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
                            >
                                No
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {/* Tasks Table */}
            <div className="overflow-x-auto">
                <table className="w-full border-collapse">
                    <thead>
                        <tr className="bg-gray-200">
                            <th className="p-2 border text-left">ID</th>
                            <th className="p-2 border text-left">Title</th>
                            <th className="p-2 border text-left">Description</th>
                            <th className="p-2 border text-left">Priority</th>
                            <th className="p-2 border text-left">Due Date</th>
                            <th className="p-2 border text-left">Status</th>
                            <th className="p-2 border text-left"></th>
                            <th className="p-2 border text-left"></th>
                        </tr>
                    </thead>
                    <tbody>
                        {tasks.map((task) => (
                            <tr key={task.taskId} className="hover:bg-gray-50">
                                <td className="p-2 border">{task.taskId}</td>
                                <td className="p-2 border">{task.title}</td>
                                <td className="p-2 border">{task.description || 'No description'}</td>
                                <td className="p-2 border">
                                    <span className={`px-2 py-1 rounded text-xs font-medium ${getPriorityColor(task.priority)}`}>
                                        {Priority[task.priority]}
                                    </span>
                                </td>
                                <td className="p-2 border">
                                    {new Date(task.dueDate).toLocaleDateString()}
                                </td>
                                <td className="p-2 border">
                                    <span className={`px-2 py-1 rounded text-xs font-medium ${getStatusColor(task.status)}`}>
                                        {Status[task.status]}
                                    </span>
                                </td>
                                <td className="p-2 border">
                                    <button
                                        onClick={() => openUpdateModal(task)}
                                        className="text-black bg-yellow-400 hover:bg-yellow-500 focus:outline-none focus:ring-4 focus:ring-yellow-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 dark:focus:ring-yellow-900"
                                    >
                                        Update
                                    </button>
                                </td>
                                <td className="p-2 border">
                                    <button
                                        onClick={() => openDeleteModal(task)}
                                        className="text-black bg-red-700 hover:bg-red-800 focus:outline-none focus:ring-4 focus:ring-red-300 font-medium rounded-full text-sm px-5 py-2.5 text-center me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default TaskGrid;