-- 焔の武僧 コウカン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.RC)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.OverDress})
end

function Activation()
	obj.Draw(1)
end