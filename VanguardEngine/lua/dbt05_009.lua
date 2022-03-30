-- 知恵の泉 エバ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.VC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("ACTCost")
	ability1.SetActivation("ACT")
	-- on attack
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnAttack)
	ability2.SetLocation(l.VC)
	ability2.SetTrigger("OnAttackTrigger")
	ability2.SetCost("OnAttackCost")
	ability2.SetCanFullyResolve("OnAttackCanFullyResolve")
	ability2.SetActivation("OnAttack")
	-- glitter
	local ability3 = NewAbility(GetID())
	ability3.SetDescription(3)
	ability3.SetProperty(p.IsGlitter)
end

function ACTCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function ACT()
	local count = obj.GetNumberOf("Filter", {l.Order})
	obj.LookAtTopOfDeck(count)
	obj.ChooseAddToHand({q.Location, l.Looking, q.Count, 1, q.Min, 0})
	obj.RearrangeOnBottom({q.Location, l.Looking})
	obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end

function Filter(id)
	return obj.IsOrderType(id, ot.Research)
end

function OnAttackTrigger()
	return obj.IsAttackingUnit()
end

function OnAttackCost(check)
	if check then return obj.CanCB(1) and obj.CanSB(1) end
	obj.CounterBlast(1)
	obj.SoulBlast(1)
end

function OnAttackCanFullyResolve()
	return obj.CanSuperiorCall({q.Location, l.Deck, q.Location, l.Hand, q.NameContains, obj.LoadName("Obscudeid"), q.Count, 1})
end

function OnAttack()
	obj.SuperiorCall({q.Location, l.Deck, q.Location, l.Hand, q.NameContains, obj.LoadName("Obscudeid"), q.Count, 1, q.Min, 0})
	obj.CheckForShuffle()
end