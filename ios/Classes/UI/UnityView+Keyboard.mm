#import "UnityView.h"
#include "UI/Keyboard.h"

static NSArray* keyboardCommands = nil;
static int pressedButton = 0;

@interface UnityView (Keyboard)
@end

@implementation UnityView (Keyboard)

- (void)createKeyboard
{
    // only English keyboard layout is supported
    NSString* baseLayout = @"1234567890-=qwertyuiop[]asdfghjkl;'\\`zxcvbnm,./!@#$%^&*()_+{}:\"|<>?~ \t\r\b\\";
    NSString* numpadLayout = @"1234567890-=*+/.\r";

    size_t sizeOfKeyboardCommands = baseLayout.length + numpadLayout.length + 11;
    NSMutableArray* commands = [NSMutableArray arrayWithCapacity: sizeOfKeyboardCommands];

    for (NSInteger i = 0; i < baseLayout.length; ++i)
    {
        NSString* input = [baseLayout substringWithRange: NSMakeRange(i, 1)];
        [commands addObject: [UIKeyCommand keyCommandWithInput: input modifierFlags: kNilOptions action: @selector(handleCommand:)]];
    }
    for (NSInteger i = 0; i < numpadLayout.length; ++i)
    {
        NSString* input = [numpadLayout substringWithRange: NSMakeRange(i, 1)];
        [commands addObject: [UIKeyCommand keyCommandWithInput: input modifierFlags: UIKeyModifierNumericPad action: @selector(handleCommand:)]];
    }

    // up, down, left, right, esc
    [commands addObject: [UIKeyCommand keyCommandWithInput: UIKeyInputUpArrow modifierFlags: kNilOptions action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: UIKeyInputDownArrow modifierFlags: kNilOptions action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: UIKeyInputLeftArrow modifierFlags: kNilOptions action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: UIKeyInputRightArrow modifierFlags: kNilOptions action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: UIKeyInputEscape modifierFlags: kNilOptions action: @selector(handleCommand:)]];

    // caps Lock, shift, control, option, command
    [commands addObject: [UIKeyCommand keyCommandWithInput: @"" modifierFlags: UIKeyModifierAlphaShift action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: @"" modifierFlags: UIKeyModifierShift action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: @"" modifierFlags: UIKeyModifierControl action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: @"" modifierFlags: UIKeyModifierAlternate action: @selector(handleCommand:)]];
    [commands addObject: [UIKeyCommand keyCommandWithInput: @"" modifierFlags: UIKeyModifierCommand action: @selector(handleCommand:)]];

    keyboardCommands = commands.copy;
}

// UIKeyCommand can't handle key up events,
// So we're simulating key up event in case if any other button is pressed or UIView calls keyCommands method.
// Because of this there is no way to handle simultanious key presses.
- (void)releaseButton
{
    if (pressedButton != 0)
    {
        UnitySetKeyState(pressedButton, false);
        pressedButton = 0;
    }
}

- (NSArray*)keyCommands
{
    [self releaseButton];

    //keyCommands take controll of buttons over UITextView, that's why need to return nil if text input field is active
    if ([[KeyboardDelegate Instance] status] == Visible)
    {
        return nil;
    }

    if (keyboardCommands == nil)
    {
        [self createKeyboard];
    }
    return keyboardCommands;
}

- (bool)isValidCodeForButton:(int)code
{
    return (code > 0 && code < 128);
}

- (void)handleCommand:(UIKeyCommand *)command
{
    [self releaseButton];

    NSString* input = command.input;
    UIKeyModifierFlags modifierFlags = command.modifierFlags;

    char inputChar = ([input length] > 0) ? [input characterAtIndex: 0] : 0;
    int code = (int)inputChar; // ASCII code

    if (![self isValidCodeForButton: code])
    {
        code = 0;
    }

    if ((modifierFlags & UIKeyModifierAlphaShift) != 0)
        code = UnityStringToKey("caps lock");
    if ((modifierFlags & UIKeyModifierShift) != 0)
        code = UnityStringToKey("left shift");
    if ((modifierFlags & UIKeyModifierControl) != 0)
        code = UnityStringToKey("left ctrl");
    if ((modifierFlags & UIKeyModifierAlternate) != 0)
        code = UnityStringToKey("left alt");
    if ((modifierFlags & UIKeyModifierCommand) != 0)
        code = UnityStringToKey("left cmd");

    if ((modifierFlags & UIKeyModifierNumericPad) != 0)
    {
        switch (inputChar)
        {
            case '0':
                code = UnityStringToKey("[0]");
                break;
            case '1':
                code = UnityStringToKey("[1]");
                break;
            case '2':
                code = UnityStringToKey("[2]");
                break;
            case '3':
                code = UnityStringToKey("[3]");
                break;
            case '4':
                code = UnityStringToKey("[4]");
                break;
            case '5':
                code = UnityStringToKey("[5]");
                break;
            case '6':
                code = UnityStringToKey("[6]");
                break;
            case '7':
                code = UnityStringToKey("[7]");
                break;
            case '8':
                code = UnityStringToKey("[8]");
                break;
            case '9':
                code = UnityStringToKey("[9]");
                break;
            case '-':
                code = UnityStringToKey("[-]");
                break;
            case '=':
                code = UnityStringToKey("equals");
                break;
            case '*':
                code = UnityStringToKey("[*]");
                break;
            case '+':
                code = UnityStringToKey("[+]");
                break;
            case '/':
                code = UnityStringToKey("[/]");
                break;
            case '.':
                code = UnityStringToKey("[.]");
                break;
            case '\r':
                code = UnityStringToKey("enter");
                break;
            default:
                break;
        }
    }

    if (input == UIKeyInputUpArrow)
        code = UnityStringToKey("up");
    else if (input == UIKeyInputDownArrow)
        code = UnityStringToKey("down");
    else if (input == UIKeyInputRightArrow)
        code = UnityStringToKey("right");
    else if (input == UIKeyInputLeftArrow)
        code = UnityStringToKey("left");
    else if (input == UIKeyInputEscape)
        code = UnityStringToKey("escape");

    UnitySetKeyState(code, true);
    pressedButton = code;
}

#if PLATFORM_TVOS
- (int)pressTypeToCode:(UIPress *)press
{
    if ([press type] == UIPressTypeUpArrow)
        return UnityStringToKey("up");
    else if ([press type] == UIPressTypeDownArrow)
        return UnityStringToKey("down");
    else if ([press type] == UIPressTypeRightArrow)
        return UnityStringToKey("right");
    else if ([press type] == UIPressTypeLeftArrow)
        return UnityStringToKey("left");
    else if ([press type] == UIPressTypeSelect)
        return UnityStringToKey("joystick button 14");
    else if ([press type] == UIPressTypePlayPause)
        return UnityStringToKey("joystick button 15");
    else if ([press type] == UIPressTypeMenu)
        return UnityStringToKey("joystick button 0");
    return 0;
}

- (void)pressesBegan:(NSSet<UIPress *> *)presses withEvent:(UIPressesEvent *)event
{
    for (UIPress *press in presses)
    {
        UnitySetKeyState([self pressTypeToCode: press], true);
    }
}

- (void)pressesEnded:(NSSet<UIPress *> *)presses withEvent:(UIPressesEvent *)event
{
    for (UIPress *press in presses)
    {
        UnitySetKeyState([self pressTypeToCode: press], false);
    }
}

#endif

@end
