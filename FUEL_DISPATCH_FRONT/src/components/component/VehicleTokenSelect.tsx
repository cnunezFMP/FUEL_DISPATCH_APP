const VehicleTokenSelect = () => (
  <>
    <label
      htmlFor="countries_disabled"
      className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
    >
      Select an option
    </label>
    <select
      id="countries_disabled"
      className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 mb-4"
    >
      <option selected>Choose a Token</option>
      <option value="F-01">F-01</option>
      <option value="F-02">F-02</option>
      <option value="F-03">F-03</option>
      <option value="F-04">F-04</option>
    </select>
  </>
);

export default VehicleTokenSelect;
