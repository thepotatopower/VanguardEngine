-- Spiritual Body Condensation

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 7
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Drop, q.Grade, 0, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Grade, 0, q.Grade, 1, q.Count, 1
	elseif n == 3 then
		return q.Location, l.Drop, q.Grade, 0, q.Grade, 1, q.Grade, 2, q.Count, 1
	elseif n == 4 then 
		return q.Location, l.Drop, q.Grade, 0, q.Grade, 1, q.Grade, 2, q.Grade, 3, q.Count, 1
	elseif n == 5 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 6 then
		return q.Location, l.Drop, q.Count, 1
	elseif n == 7 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.AlchemagicSB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanSB(5) then 
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.Exists(6) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(5)
	end
end

function Activate(n)
	if n == 1 then
		local g = obj.VanguardGrade()
		if g == 0 then
			obj.Select(1)
		elseif g == 1 then
			obj.Select(2)
		elseif g == 2 then
			obj.Select(3)
		elseif g == 3 then
			obj.Select(4)
		end
		obj.SuperiorCall(7)
		obj.AddTempPower(7, 5000)
	end
	return 0
end