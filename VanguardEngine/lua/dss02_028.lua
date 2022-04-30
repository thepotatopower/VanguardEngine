-- 四面恩光の女魔術師

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.GetNumberOf("Filter", {l.UnitsHit}) > 0
end

function Filter(id)
	return not obj.IsPlayer(id) and obj.IsVanguard(id)
end

function Condition()
	return obj.IsBoosted()
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function Activation()
	obj.Draw(1)
end