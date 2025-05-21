import React from 'react';
import { useState } from 'react';

const Select = ({
  options,
  onSelect,
  className = "",
  label,
  name = "",
  required = false,
  disabled = false,
  defaultValue = "",
}) => {
  const [selected, setSelected] = useState(defaultValue);

  const handleChange = (event) => {
    const value = event.target.value;
    onSelect(value);
    setSelected(value);
  };

  return (
    <div className={className}>
      <label htmlFor={name}>{label}</label>
      <select
        disabled={disabled}
        required={required}
        onChange={handleChange}
        name={name}
        id={name}
        value={selected}
      >
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </div>
  );
};

export default Select;