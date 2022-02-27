-- 溶変怪獣 オルシディラン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Activation()
	obj.Draw(1)
	obj.Discard(1)
end