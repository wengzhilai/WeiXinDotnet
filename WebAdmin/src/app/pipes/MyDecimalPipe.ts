import { Inject, LOCALE_ID, Pipe, PipeTransform, Type } from '@angular/core';
import { NumberFormatStyle, NumberFormatter } from './NumberFormat';

const _NUMBER_FORMAT_REGEXP = /^(\d+)?\.((\d+)(-(\d+))?)?$/;

function formatNumber(
  pipe: Type<any>, locale: string, value: number | string, style: NumberFormatStyle,
  digits?: string | null, currency: string | null = null,
  currencyAsSymbol: boolean = false): string | null {
  if (value == null) return null;

  // Convert strings to numbers
  value = typeof value === 'string' && isNumeric(value) ? +value : value;
  if (typeof value !== 'number') {
    return null;
  }

  let minInt: number | undefined = undefined;
  let minFraction: number | undefined = undefined;
  let maxFraction: number | undefined = undefined;
  if (style !== NumberFormatStyle.Currency) {
    // rely on Intl default for currency
    minInt = 1;
    minFraction = 0;
    maxFraction = 3;
  }

  if (digits) {
    const parts = digits.match(_NUMBER_FORMAT_REGEXP);
    if (parts === null) {
      throw new Error(`${digits} is not a valid digit info for number pipes`);
    }
    if (parts[1] != null) {  // min integer digits
      minInt = parseIntAutoRadix(parts[1]);
    }
    if (parts[3] != null) {  // min fraction digits
      minFraction = parseIntAutoRadix(parts[3]);
    }
    if (parts[5] != null) {  // max fraction digits
      maxFraction = parseIntAutoRadix(parts[5]);
    }
  }

  return NumberFormatter.format(value as number, locale, style, {
    minimumIntegerDigits: minInt,
    minimumFractionDigits: minFraction,
    maximumFractionDigits: maxFraction,
    currency: currency,
    currencyAsSymbol: currencyAsSymbol,
  });
}

/**
{minIntegerDigits}.{minFractionDigits}-{maxFractionDigits}
minIntegerDigits is the minimum number of integer digits to use. Defaults to 1.
minFractionDigits is the minimum number of digits after fraction. Defaults to 0.
maxFractionDigits is the maximum number of digits after fraction. Defaults to 3.
 * 
 * @export
 * @class MyDecimalPipe
 * @implements {PipeTransform}
 */
@Pipe({ name: 'myNumber' })
export class MyDecimalPipe implements PipeTransform {
  constructor( @Inject(LOCALE_ID) private _locale: string) { }

  transform(value: any, digits?: string): string | null {
    let reStr=formatNumber(MyDecimalPipe, this._locale, value, NumberFormatStyle.Decimal, digits);
    if(reStr==null)
    {
      return "";
    }
    return reStr.replace(/,/g,"");
  }
}


@Pipe({ name: 'Mypercent' })
export class MyPercentPipe implements PipeTransform {
  constructor( @Inject(LOCALE_ID) private _locale: string) { }

  transform(value: any, digits?: string): string | null {
    return formatNumber(MyPercentPipe, this._locale, value, NumberFormatStyle.Percent, digits);
  }
}


@Pipe({ name: 'Mycurrency' })
export class MyCurrencyPipe implements PipeTransform {
  constructor( @Inject(LOCALE_ID) private _locale: string) { }

  transform(
    value: any, currencyCode: string = 'CNY', symbolDisplay: boolean = false,
    digits?: string): string | null {
    var reStr = formatNumber(
      MyCurrencyPipe, this._locale, value, NumberFormatStyle.Currency, digits, currencyCode,
      symbolDisplay);
    if (reStr != null && reStr != '') {
      reStr = reStr.replace("CN", "");
    }
    return reStr;
  }
}

function parseIntAutoRadix(text: string): number {
  const result: number = parseInt(text);
  if (isNaN(result)) {
    throw new Error('Invalid integer literal when parsing ' + text);
  }
  return result;
}

export function isNumeric(value: any): boolean {
  return !isNaN(value - parseFloat(value));
}