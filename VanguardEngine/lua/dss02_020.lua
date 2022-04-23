-- 柩機の獣 ダダイ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.IsAbyssalDarkNight()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.SuperiorCall({}, {}, 1, 1, "dbt01_t01", {}, {})
end