-- Vairina Valiente

function NumberOfAbilities()
	return 3
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Name, "Trickstar"
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Grade, 3, q.Grade, 2, q.Grade, 1, q.Grade, 0, q.Other, o.OverDress
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 4 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Hand, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OverDress, 1, 2
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 3 then
		return a.OnAttackHits, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 2 then
		if obj.InOverDress() then
			return true
		end
	elseif n == 3 then
		if not obj.Activated() and obj.IsAttackingUnit() and obj.InOverDress() and obj.CanCB(4) and obj.CanDiscard(5) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 2 then
		return true
	elseif n == 3 then
		return true
	end
	return false
end

function Cost(n)
	if n == 3 then
		obj.CounterBlast(4)
		obj.Discard(5)
	end
end

function Activate(n)
	if n == 2 then
		obj.SetAbilityPower(3, obj.NumOriginalDress() * 5000)
	elseif n == 3 then
		obj.Stand(3)
	end
	return 0
end
