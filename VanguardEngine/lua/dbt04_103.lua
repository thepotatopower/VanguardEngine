-- 天救の御旗 サリーネ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnStand)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable() and obj.SourceIsVanguardAbility()
end

function Activation()
	obj.SoulCharge(1)
end