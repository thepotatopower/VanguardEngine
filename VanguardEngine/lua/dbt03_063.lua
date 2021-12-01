-- 焔の拳僧 テンジ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player})
end

function Condition()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player, q.Other, o.OverDress})
end

function Activation()
	obj.Draw(1)
	obj.Discard(1)
end