-- Flame Dragon Bomber

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false
	elseif n == 2 then
		return a.Cont, t.Cont, p.HasPrompt, false, p.IsMandatory, true
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	elseif n == 2 then
		if obj.IsVanguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
end

function Activate(n)
	if n == 1 then
		obj.ChooseGiveAbility(1, 2)
	elseif n == 2 then
		obj.AllowAttackingBackRow(2)
	end
	return 0
end