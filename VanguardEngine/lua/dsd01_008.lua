-- 護衛忍竜 ハヤシカゼ

function RegisterAbilities()
	-- on put
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PutOnGC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanDiscard(1) end
	obj.Discard(1)
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardState({q.Location, l.Selected}, cs.CannotBeHit, p.UntilEndOfBattle)
end