-- 極光戦姫 サーヴェ・バーミリオン

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetLocation(l.RC)
	ability1.SetMovedFrom(l.Order)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Enemy})
end

function Activation()
	return obj.SoulCharge(1)
end