/**
 * react-hook-form carries the message with the constraint, so components read
 * `errors.field?.message` instead of re-deriving text from the error type.
 */
export function textRule(minLength: number, maxLength: number) {
  return {
    required: 'Your input is required',
    minLength: {
      value: minLength,
      message: `Your input is below minimum of ${minLength} characters`
    },
    maxLength: {
      value: maxLength,
      message: `Your input exceeds maximum of ${maxLength} characters`
    }
  };
}

export const NAME_RULE = textRule(3, 50);
export const SUMMARY_RULE = textRule(10, 100);
