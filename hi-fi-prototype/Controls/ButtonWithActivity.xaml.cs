namespace hi_fi_prototype.Controls;

public partial class ButtonWithActivity : ContentView
{
    public static readonly BindableProperty InitialTextProperty = BindableProperty.Create(
        nameof(InitialText), typeof(string), typeof(ButtonWithActivity), "Button");

    public static readonly BindableProperty SuccessResultTextProperty = BindableProperty.Create(
        nameof(SuccessResultText), typeof(string), typeof(ButtonWithActivity), "Success");

    public static readonly BindableProperty FailureResultTextProperty = BindableProperty.Create(
        nameof(FailureResultText), typeof(string), typeof(ButtonWithActivity), "Failure");

    public static readonly BindableProperty AnimationBaseTimeMsProperty = BindableProperty.Create(
        nameof(AnimationBaseTime), typeof(uint), typeof(ButtonWithActivity), 1000U);

    public static readonly BindableProperty AnimationMinTimeMsProperty = BindableProperty.Create(
        nameof(AnimationMinTime), typeof(uint), typeof(ButtonWithActivity), 2000U);

    public static readonly BindableProperty AnimationResultTimeMsProperty = BindableProperty.Create(
        nameof(AnimationResultTimeMs), typeof(int), typeof(ButtonWithActivity), 1000);

    public static readonly BindableProperty AnimationRateProperty = BindableProperty.Create(
        nameof(AnimationRate), typeof(uint), typeof(ButtonWithActivity), 16U);

    public static readonly BindableProperty AnimationYMovementProperty = BindableProperty.Create(
        nameof(AnimationYMovement), typeof(double), typeof(ButtonWithActivity), 75.0);

    public static readonly BindableProperty ActivityImageSizeProperty = BindableProperty.Create(
        nameof(ActivityImageSize), typeof(int), typeof(ButtonWithActivity), 24);

    public static readonly BindableProperty ActivityImageSourceProperty = BindableProperty.Create(
        nameof(ActivityImageSource), typeof(string), typeof(ButtonWithActivity), "settings.png");

    public static readonly BindableProperty ForegroundColorProperty = BindableProperty.Create(
        nameof(ForegroundColor), typeof(Color), typeof(ButtonWithActivity), Colors.White);

    public static readonly BindableProperty ActivityParameterProperty = BindableProperty.Create(
        nameof(ActivityParameter), typeof(object), typeof(ButtonWithActivity), null);

    public ButtonWithActivity()
    {
        InitializeComponent();
        BindingContext = this;

        Starting = (sender, args) => { };
        Started = (sender, args) => { };
        Stopping = (sender, args) => { };

        Activity = (parameter) => { return (Task<bool>)Task.CompletedTask; };
        ActivitySucceeded = (sender, args) => { };
        ActivityFailed = (sender, args) => { };
    }

    public event EventHandler Starting;
    public event EventHandler Started;
    public event EventHandler Stopping;
    public event EventHandler ActivitySucceeded;
    public event EventHandler ActivityFailed;

    public Func<object, Task<bool>> Activity;

    public string FailureExceptionText { get; private set; } = string.Empty;

    public string InitialText
    {
        get => (string)GetValue(InitialTextProperty);
        set => SetValue(InitialTextProperty, value);
    }

    public string SuccessResultText
    {
        get => (string)GetValue(SuccessResultTextProperty);
        set => SetValue(SuccessResultTextProperty, value);
    }

    public string FailureResultText
    {
        get => (string)GetValue(FailureResultTextProperty);
        set => SetValue(FailureResultTextProperty, value);
    }

    public uint AnimationBaseTime
    {
        get => (uint)GetValue(AnimationBaseTimeMsProperty);
        set => SetValue(AnimationBaseTimeMsProperty, value);
    }

    public uint AnimationMinTime
    {
        get => (uint)GetValue(AnimationMinTimeMsProperty);
        set => SetValue(AnimationMinTimeMsProperty, value);
    }

    public int AnimationResultTimeMs
    {
        get => (int)GetValue(AnimationResultTimeMsProperty);
        set => SetValue(AnimationResultTimeMsProperty, value);
    }

    public uint AnimationRate
    {
        get => (uint)GetValue(AnimationRateProperty);
        set => SetValue(AnimationRateProperty, value);
    }

    public double AnimationYMovement
    {
        get => (double)GetValue(AnimationYMovementProperty);
        set => SetValue(AnimationYMovementProperty, value);
    }

    public int ActivityImageSize
    {
        get => (int)GetValue(ActivityImageSizeProperty);
        set => SetValue(ActivityImageSizeProperty, value);
    }

    public string ActivityImageSource
    {
        get => (string)GetValue(ActivityImageSourceProperty);
        set => SetValue(ActivityImageSourceProperty, value);
    }

    public Color ForegroundColor
    {
        get => (Color)GetValue(ForegroundColorProperty);
        set => SetValue(ForegroundColorProperty, value);
    }

    public object ActivityParameter
    {
        get => GetValue(ActivityParameterProperty);
        set => SetValue(ActivityParameterProperty, value);
    }

    private const string ANIM_NAME = "buttonWithActivity";

    private Color _opaqueTextColor = Colors.White;
    private Color _transparentTextColor = Colors.Transparent;
    private string _initialButtonText = string.Empty;
    private bool _isActivityCompleted;
    private readonly Thickness _middleOfButton = new(0);

    private async void StartActivityButton_Clicked(object sender, EventArgs e)
    {
        Starting.Invoke(this, e);

        SetInitialState();
        StartMovingInitialTextOut();

        await MoveActivityIndicatorIn();
        StartIndicatingActivity();

        bool success = true;
        try
        {
            var startTime = DateTime.Now;

            FailureExceptionText = string.Empty;
            _isActivityCompleted = false;
            Started.Invoke(this, e);

            success = await Activity(ActivityParameter);

            var elapsedTime = DateTime.Now - startTime;
            if (elapsedTime.Milliseconds < AnimationMinTime)
                await Task.Delay((int)(AnimationMinTime - elapsedTime.Milliseconds));
        }
        catch (Exception ex)
        {
            FailureExceptionText = ex.ToString();
            success = false;
        }
        finally
        {
            _isActivityCompleted = true;
            Stopping.Invoke(this, e);
        }

        StartMovingResultTextIn(success);
        await MoveActivityIndicatorOut();

        await Task.Delay(AnimationResultTimeMs); // Give user time to read result text

        await FadeResultTextOut();
        await FadeInitialTextIn();

        if (success)
            ActivitySucceeded.Invoke(this, e);
        else
            ActivityFailed.Invoke(this, e);
    }

    private void SetInitialState()
    {
        _opaqueTextColor = new(StartActivityButton.TextColor.Red, StartActivityButton.TextColor.Green, StartActivityButton.TextColor.Blue, 1.0f);
        _transparentTextColor = new(StartActivityButton.TextColor.Red, StartActivityButton.TextColor.Green, StartActivityButton.TextColor.Blue, 0.0f);

        ActivityImage.Rotation = 0.0;
        ActivityImage.IsVisible = true;
    }

    private void StartMovingInitialTextOut()
    {
        StartActivityButton.TextColor = _opaqueTextColor;
        _ = StartActivityButton.TextColorTo(_transparentTextColor, AnimationRate, AnimationBaseTime, Easing.CubicOut); // Text fades out

        StartActivityButton.Padding = _middleOfButton;
        TranslateTextFromMiddleToAbove(y => StartActivityButton.Padding = new Thickness(0, y, 0, 0)); // Text moves from middle to above
    }

    private async Task MoveActivityIndicatorIn()
    {
        ActivityImage.TranslationY = AnimationYMovement; // below button
        var a0 = ActivityImage.TranslateTo(0.0, 0.0, AnimationBaseTime, Easing.CubicOut); // Image moves from below to middle

        ActivityImage.Opacity = 0.0;
        var a1 = ActivityImage.FadeTo(1.0, AnimationBaseTime / 2, Easing.CubicIn); // Image fades in

        await Task.WhenAll(a0, a1);
    }

    private void StartIndicatingActivity()
    {
        new Animation(x => ActivityImage.Rotation = x, 0, 180)
            .Commit(this, ANIM_NAME, AnimationRate, AnimationMinTime,
                    Easing.CubicInOut, null, () => { return !_isActivityCompleted; });
    }

    private void StartMovingResultTextIn(bool success)
    {
        _initialButtonText = new(StartActivityButton.Text); // Save the text, then change it
        StartActivityButton.Text = success ? SuccessResultText : FailureResultText;

        StartActivityButton.TextColor = _transparentTextColor;
        _ = StartActivityButton.TextColorTo(_opaqueTextColor, AnimationRate, AnimationBaseTime, Easing.CubicOut); // Text fades in

        StartActivityButton.Padding = new Thickness(0, 0, 0, -AnimationYMovement); // below button
        TranslateTextFromBelowToMiddle(y => StartActivityButton.Padding = new Thickness(0, 0, 0, y)); // Text moves from below to middle
    }

    private async Task MoveActivityIndicatorOut()
    {
        ActivityImage.TranslationY = 0; // middle of button
        var a0 = ActivityImage.TranslateTo(0.0, -AnimationYMovement, AnimationBaseTime, Easing.SinOut); // Image moves from middle to above

        ActivityImage.Opacity = 1.0;
        var a1 = ActivityImage.FadeTo(0.0, AnimationBaseTime / 2, Easing.CubicOut); // Image fades out

        await Task.WhenAll(a0, a1);

        ActivityImage.IsVisible = false;
    }

    private async Task FadeResultTextOut()
    {
        await StartActivityButton.TextColorTo(_transparentTextColor, AnimationRate, AnimationBaseTime, Easing.CubicOut); // Text fades out
    }

    private async Task FadeInitialTextIn()
    {
        StartActivityButton.Text = _initialButtonText; // Restore the text
        await StartActivityButton.TextColorTo(_opaqueTextColor, AnimationRate, AnimationBaseTime, Easing.CubicOut); // Text fades in
    }

    private Animation TranslateTextFromMiddleToAbove(Action<double> callback)
    {
        var a = new Animation(callback, 0, -AnimationYMovement);
        a.Commit(this, ANIM_NAME, AnimationRate, AnimationBaseTime, Easing.SinOut);
        return a;
    }

    private Animation TranslateTextFromBelowToMiddle(Action<double> callback)
    {
        var a = new Animation(callback, -AnimationYMovement, 0);
        a.Commit(this, ANIM_NAME, AnimationRate, AnimationBaseTime, Easing.CubicOut);
        return a;
    }
}
