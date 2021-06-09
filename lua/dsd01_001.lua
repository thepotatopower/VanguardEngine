-- Chakrabarthi Divine Dragon, Nirvana

function NumberOfEffects()
	return 2
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1
		return q.Count, 1
	else if n == 2
		return q.Location, l.Drop, q.Grade, 0
	else if n == 3
		return q.Location, l.PlayerRC, q.Type, t.overDress
	else if n == 4
		return q.Location, l.PlayerVC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, l.VG, false, false
	else if n == 2 then
		return a.OnAttack, l.VG, false, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if !obj.Activated(n) and obj.IsVanguard() and obj.IsAttackingUnit() and obj.CanDiscard(1) and obj.CanSuperiorCall(2) then
			return true
		else if obj.IsVanguard() and obj.IsAttackingUnit() and obj.CanCB(1) 
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.Discard(1)
		obj.SuperiorCall(2)
	else if n == 2 then
		obj.CounterBlast(1)
		obj.AddTempPower(3, 10000)
		obj.AddTempPower(4, 10000)
	end
end