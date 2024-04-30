import VehicleTokenSelect from "./VehicleTokenSelect";

function RegistrationForm() {
  // Array of objects containing information about each input field
  const inputFields = [
    // {
    //   id: "vehicle_token",
    //   label: "Vehicle Token",
    //   type: "text",
    //   placeholder: "F-00",
    //   required: true,
    // },
    {
      id: "driver_id",
      label: "Driver Id",
      type: "text",
      placeholder: "5",
      required: true,
    },
    {
      id: "road_id",
      label: "Road",
      type: "text",
      placeholder: "2",
      required: true,
    },
    {
      id: "request_id",
      label: "Request Id",
      type: "text",
      placeholder: "2",
      required: false,
    },
    {
      id: "dispenser_id",
      label: "Dispenser Id",
      type: "select",
      placeholder: "1",
      required: true,
    },
  ];

  const inputFieldsO = [
    {
      id: "Odometer",
      label: "Car Odometer",
      type: "number",
      placeholder: "Current car odometer",
      required: true,
    },
    {
      id: "gallons",
      label: "Fuel Gallons For Dispatch",
      type: "number",
      placeholder: "7",
      required: true,
    },
    {
      id: "branch_office",
      label: "Branch Office",
      type: "number",
      placeholder: "1",
      required: true,
    },
    {
      id: "fuel_calc",
      label: "Consuption Calc",
      type: "number",
    },
  ];

  return (
    <form className="grid mx-auto max-w-3xl mt-8">
          <VehicleTokenSelect/>
        <div className="grid grid-cols-2 gap-5">
        {inputFields.map((field, index) => (
          <div key={index} className="mb-6">
            <label
              htmlFor={field.id}
              className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
            >
              {field.label}
            </label>
            <input
              type={field.type}
              id={field.id}
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder={field.placeholder}
              required={field.required}
            />
          </div>
        ))}
      </div>

      <div className="grid grid-cols-2 gap-5">
        {inputFieldsO.map((field, index) => (
          <div key={index} className="mb-6 grid">
            <label
              htmlFor={field.id}
              className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
            >
              {field.label}
            </label>
            <input
              type={field.type}
              id={field.id}
              className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder={field.placeholder}
              required={field.required}
              readOnly={field.id === "fuel_calc"}
            />
          </div>
        ))}
      </div>

      <div className="flex items-start mb-6">
        <div className="flex items-center h-5">
          
        </div>
        <label
          htmlFor="remember"
          className="ms-2 text-sm font-medium text-gray-900 dark:text-gray-300"
        ></label>
      </div>
      <div>
        <label
          htmlFor="message"
          className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
        >
          Notes
        </label>
        <textarea
          id="message"
          rows={4}
          className="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border 
          border-gray-300 focus:ring-blue-500 
          focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 
          dark:placeholder-gray-400 dark:text-white 
          dark:focus:ring-blue-500 dark:focus:border-blue-500 mb-3"
          
          placeholder="Any Note?"
        ></textarea>
        
      </div>
      <button
        type="submit"
        className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
      >
        Submit
      </button>
    </form>
  );
}

export default RegistrationForm;
