<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Xaml.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationCore.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationProvider.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\UIAutomationTypes.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\ReachFramework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\PresentationUI.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\System.Printing.dll</Reference>
  <Namespace>System.Windows.Forms</Namespace>
  <Namespace>System.Windows.Controls</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

int buttonClicks = 0;
int numberOfClickers = 0;

int autoClickerCost = 10;

Button clickButton;
Button buyClickerButton;
Label clicksLabel;
Label clickersLabel;

void Main()
{
	clickButton = new Button() { Content = "Click Me", Width = 100, HorizontalAlignment = System.Windows.HorizontalAlignment.Left };
	clickButton.Click += ClickButton_Click;

	buyClickerButton = new Button { Content = string.Format("Buy Clicker ({0})", autoClickerCost), Width = 100, HorizontalAlignment = HorizontalAlignment.Left, Visibility = Visibility.Hidden };
	buyClickerButton.Click += BuyAutoClickerButton_Click;
	
	clicksLabel = new Label() { Content = "Clicks: 0" };
	clickersLabel = new Label() { Content = "AutoClickers: 0" };
	
	PanelManager.StackWpfElement(clicksLabel, "ClickGame");
	PanelManager.StackWpfElement(clickersLabel, "ClickGame");
	PanelManager.StackWpfElement(clickButton, "ClickGame");
	PanelManager.StackWpfElement(buyClickerButton, "ClickGame");
	
	AutoClick();
}

private void ClickButton_Click(object sender, RoutedEventArgs e)
{
	ButtonClick();
}

private void ButtonClick()
{
	buttonClicks++;

	clicksLabel.Content = "Clicks: " + buttonClicks;

	if (buttonClicks == 10)
	{
		buyClickerButton.Visibility = Visibility.Visible;
	}
}

private void BuyAutoClickerButton_Click(object sender, RoutedEventArgs e)
{
	if (buttonClicks >= autoClickerCost)
	{
		BuyAutoClicker();
	}
}

private void BuyAutoClicker()
{
	buttonClicks -= autoClickerCost;
	autoClickerCost = (int)(1.2f * autoClickerCost);
	
	numberOfClickers++;
	
	clicksLabel.Content = string.Format("Clicks: {0}", buttonClicks);
	clickersLabel.Content = string.Format("AutoClickers: {0}", numberOfClickers);
	buyClickerButton.Content = string.Format("Buy Clicker ({0})", autoClickerCost);
}

async private void AutoClick()
{
	while (true)
	{
		buttonClicks += numberOfClickers;
		clicksLabel.Content = string.Format("Clicks: {0}", buttonClicks);
		await Task.Delay(1000);
	}
}

// Define other methods and classes here
