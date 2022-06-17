type FieldMask = string[];

function getFieldMask(resource: Object): string[] {
  const fieldMask: FieldMask = [];

  for (const [key, value] of Object.entries(resource)) {
    if (value instanceof Object) {
      for (const field of getFieldMask(value)) {
        fieldMask.push(`${key}.${field}`);
      }
    } else if (value !== undefined) {
      fieldMask.push(key);
    }
  }

  return fieldMask;
}
export default getFieldMask;
