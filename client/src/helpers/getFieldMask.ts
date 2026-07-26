export type FieldMask = string[];

function isPlainObject(value: unknown): value is Record<string, unknown> {
  return (
    typeof value === 'object' &&
    value !== null &&
    !Array.isArray(value) &&
    !(value instanceof Date)
  );
}

/**
 * Builds the `FieldMask` the API uses to decide which properties a PATCH
 * touches. Arrays and dates are leaves — recursing into them would emit
 * index paths the server cannot resolve.
 */
function getFieldMask(resource: Record<string, unknown>): FieldMask {
  const fieldMask: FieldMask = [];

  for (const [key, value] of Object.entries(resource)) {
    if (value === undefined) continue;

    if (isPlainObject(value)) {
      for (const field of getFieldMask(value)) {
        fieldMask.push(`${key}.${field}`);
      }
    } else {
      fieldMask.push(key);
    }
  }

  return fieldMask;
}

export default getFieldMask;
