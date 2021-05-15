--Chakrabarthi Divine Dragon, Nirvana

function NumberOfEffects()
	return 2
end

function NumberOfParams(n)
	if n == 1
		return 2
	else if n == 2
		return 1
	end
end

function GetParam(n, m)
	if n == 1
		if m == 1
			return q.Count, 1
		else if m == 2
			return q.Location, l.Drop, q.Grade, 0
		end
	if n == 2
		if m == 1
			return q.Count, 1
		end
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, l.VG, false, false
	else if n == 2 then
		return a.OnAttack, l.VG, false, false
	end
	return 0
end

function CheckCondition(n)
	if n == 1 then
		if !obj.Activated(n) and obj.IsVanguard() and obj.IsAttackingUnit() and obj.CanDiscard(1) and obj.CanSuperiorCall(2) then
			return true
		else if obj.IsVanguard() and obj.IsAttackingUnit() and obj.CanCB() 
			return true
		end
	end
	return false
end

function Activate(n, i, j)
	if n == 1 then
		obj.Discard(1)
		obj.SuperiorCall(2)
	end
end