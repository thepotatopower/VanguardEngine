-- Moment of Securing! Aurora Battle Princesses 24 Hours Coverage!

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 5
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyHand, q.Count, 4
	elseif n == 3 then
		return q.Location, l.EnemyHand, q.Count, 1
	elseif n == 4 then
		return q.Location, l.PlayerPrisoners, q.Count, 3
	elseif n == 5 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnOrder, t.Order, p.HasPrompt, true, p.IsMandatory, false, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.CanSB(1) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.HasPrison() and obj.Exists(2) then
			return true
		end
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.SoulBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		if obj.Exists(2) then
			obj.EnemyChooseImprison(3) 
		end
		if obj.Exists(4) then
			obj.ChooseAddTempPower(5, 5000)
		end
	end
	return 0
end