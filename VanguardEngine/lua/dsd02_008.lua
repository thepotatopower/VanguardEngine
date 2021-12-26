-- アクロバット・プレゼンター

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() or obj.IsBooster()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dsd01_001")})
end

function Activation()
	obj.SoulCharge(1)
	if obj.InFinalRush() then
		obj.SoulCharge(1)
	end
end