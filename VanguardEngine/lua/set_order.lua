-- Set Order

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetTiming(a.OnOrder)
	ability1.SetActivation("OnOrder")
end

function OnOrder()
end