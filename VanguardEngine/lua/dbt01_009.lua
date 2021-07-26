-- Mysterious Rain Spiritualist, Zorga

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.Drop, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	elseif n == 2 then
		return a.OnACT, t.ACT, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() then
			return true
		end
	elseif n == 2 then
		if obj.IsVanguard() and not obj.Activated() and obj.CanCB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 2 then
		if obj.Exists(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 2 then
		obj.CounterBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		obj.SetAlchemagicDiff()
	elseif n == 2 then
		obj.SuperiorCall(2)
	end
	return 0
end