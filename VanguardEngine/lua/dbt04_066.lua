-- 砂塵の擲弾 オーリー

function RegisterAbilities()
	-- on retire
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRetire)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.PlayerMainPhase() and obj.Exists({q.Location, l.Applicable, q.Other, o.Enemy})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt04_067")})
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.PlayerRC, q.Name, obj.GetNameFromCardID("dbt04_067"), q.Count, 1})
end