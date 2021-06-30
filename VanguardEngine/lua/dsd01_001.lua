-- Chakrabarthi Divine Dragon, Nirvana

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerHand, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Grade, 0
	elseif n == 3 then
		return q.Location, l.PlayerRC, q.Type, t.overDress
	elseif n == 4 then
		return q.Location, l.PlayerVC
	elseif n == 5 then
		return q.Location, l.Damage, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if not obj.Activated(n) and obj.IsVanguard() and obj.CanDiscard(1) and obj.CanSuperiorCall(2) then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and obj.IsAttackingUnit() and obj.CanCB(5) then 
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.Discard(1)
	elseif n == 2 then
		obj.CounterBlast(5)
	end
end

function Activate(n)
	if n == 1 then
		obj.SuperiorCall(2)
	elseif n == 2 then
		obj.AddTempPower(3, 10000)
		obj.AddTempPower(4, 10000)
	end
	return 0
end